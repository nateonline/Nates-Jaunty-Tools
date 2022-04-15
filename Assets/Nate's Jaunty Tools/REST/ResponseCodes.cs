using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NatesJauntyTools;

namespace NatesJauntyTools.REST
{
	public enum ResponseCode
	{
		// https://www.restapitutorial.com/httpstatuscodes.html
		// https://en.wikipedia.org/wiki/List_of_HTTP_status_codes

		Continue = 100,
		SwitchingProtocols = 101,
		Processing = 102,
		Checkpoint = 103,

		OK = 200,
		Created = 201,
		Accepted = 202,
		NonAuthoritativeInformation = 203,
		NoContent = 204,
		ResetContent = 205,
		PartialContent = 206,
		MultiStatus = 207,
		AlreadyReported = 208,
		ThisIsFine = 218,
		IMUsed = 226,

		MultipleChoices = 300,
		MovedPermanently = 301,
		Found = 302,
		SeeOther = 303,
		NotModified = 304,
		UseProxy = 305,
		Unused = 306,
		TemporaryRedirect = 307,
		PermanentRedirect = 308,

		BadRequest = 400,
		Unauthorized = 401,
		PaymentRequired = 402,
		Forbidden = 403,
		NotFound = 404,
		MethodNotAllowed = 405,
		NotAcceptable = 406,
		ProxyAuthenticationRequired = 407,
		RequestTimeout = 408,
		Conflict = 409,
		Gone = 410,
		LengthRequired = 411,
		PrecautionFailed = 412,
		RequestEntityTooLarge = 413,
		RequestUriTooLong = 414,
		UnsupportedMediaType = 415,
		RequestedRangeNotSatisfiable = 416,
		ExpectationFailed = 417,
		ImATeapot = 418,
		PageExpired = 419,
		EnhanceYourCalm = 420,
		MisdirectedRequest = 421,
		UnprocessableEntity = 422,
		Locked = 423,
		FailedDependency = 424,
		TooEarly = 425,
		UpgradeRequired = 426,
		PrecautionRequired = 428,
		TooManyRequests = 429,
		RequestHeaderFieldsTooLargeShopify = 430,
		RequestHeaderFieldsTooLarge = 431,
		BlockedByWindowsParentalControls = 450,
		UnavailableForLegalReasons = 451,
		InvalidToken = 498,
		TokenRequired = 499,

		InternalServerError = 500,
		NotImplemented = 501,
		BadGateway = 502,
		ServiceUnavailable = 503,
		GatewayTimeout = 504,
		HttpVersionNotSupported = 505,
		VariantAlsoNegotiates = 506,
		InsufficientStorage = 507,
		LoopDetected = 508,
		BandwidthLimitExceeded = 509,
		NotExtended = 510,
		NetworkAuthenticationRequired = 511,
		SiteIsOverloaded = 529,
		SiteIsFrozen = 530,
		NetworkReadTimeoutError = 598,
	}

	public static class ResponseCodeExtensions
	{
		public static string Name(this long code)
		{
			return ((ResponseCode)code).Name();
		}

		public static string Name(this ResponseCode code)
		{
			return Tools.SplitCamelCase(code.ToString());
		}

		public static bool IsSuccess(this ResponseCode responseCode)
		{
			return (200 <= (int)responseCode && (int)responseCode <= 299);
		}

		public static ResponseCode ToResponseEnum(this long code)
		{
			return (ResponseCode)code;
		}

		public static ResponseCode ResponseCodeEnum(this UnityWebRequest request)
		{
			return request.responseCode.ToResponseEnum();
		}
	}
}
