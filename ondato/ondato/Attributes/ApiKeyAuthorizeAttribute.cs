using Microsoft.AspNetCore.Mvc;
using ondato.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ondato.Attributes
{
	public class ApiKeyAuthorizeAttribute : TypeFilterAttribute
	{
		public ApiKeyAuthorizeAttribute() : base(typeof(ApiKeyAuthorizeAsyncFilter))
		{
		}
	}
}
