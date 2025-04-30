using WebApi.Common;

namespace WebApi.Services;

public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor)
{
    private const string TenantIdHeaderName = "X-Tenant-Id";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public int GetTenantId()
    {
        var tenantIdHeader = _httpContextAccessor.HttpContext?.Request.Headers[TenantIdHeaderName];

        if (!tenantIdHeader.HasValue ||
            !int.TryParse(tenantIdHeader.Value, out int tenantId) ||
            Tenants.All.Contains(tenantId))
        {
            throw new ApplicationException("Invalid tenant id");
        }

        return tenantId;
    }
}