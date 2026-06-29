using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using QuizUpLearn.API.Models;

namespace QuizUpLearn.API.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.StartsWithSegments("/game-hub") ||
			    context.Request.Path.StartsWithSegments("/one-vs-one-hub") ||
				context.Request.Path.StartsWithSegments("/background-jobs"))
			{
				await _next(context);
				return;
			}

			try
			{
				await _next(context);
			}
			catch (HttpException ex)
			{
				_logger.LogWarning(ex, "HttpException {StatusCode} {Path}", ex.StatusCode, context.Request.Path);
				await WriteErrorAsync(context, ex.StatusCode, ex.Message, ex.ErrorType);
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogWarning(ex, "UnauthorizedAccessException {Path}", context.Request.Path);
				await WriteErrorAsync(context, HttpStatusCode.Unauthorized, ex.Message, ApiErrorType.Unauthorized);
			}
			catch (KeyNotFoundException ex)
			{
				_logger.LogWarning(ex, "KeyNotFoundException {Path}", context.Request.Path);
				await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message, ApiErrorType.BadRequest);
			}
			catch (ArgumentException ex)
			{
				_logger.LogWarning(ex, "ArgumentException {Path}", context.Request.Path);
				await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message, ApiErrorType.BadRequest);
			}
			catch (TimeoutException ex)
			{
				_logger.LogError(ex, "TimeoutException {Path}", context.Request.Path);
				await WriteErrorAsync(context, HttpStatusCode.InternalServerError, ex.Message, ApiErrorType.InternalServerError);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception {Method} {Path}", context.Request.Method, context.Request.Path);
				await WriteErrorAsync(context, HttpStatusCode.InternalServerError, ex.Message, ApiErrorType.InternalServerError);
			}
		}

		private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message, ApiErrorType errorType)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			var response = ApiResponse<object>.Fail(message, errorType);
			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
	}
}

