using Backend.Application.Dtos;

namespace Backend.Presentation.Hypermedia;

public static class LinkFactory
{
    public static List<LinkDto> ForShop(Guid shopId)
    {
        var self = $"/api/shops/{shopId}";

        return new List<LinkDto>
        {
            new LinkDto { Rel = "self", Href = self, Method = "GET" },
            new LinkDto { Rel = "update", Href = self, Method = "PUT" },
            new LinkDto { Rel = "delete", Href = self, Method = "DELETE" },
            new LinkDto { Rel = "products", Href = $"{self}/products", Method = "GET" },
            new LinkDto { Rel = "summary", Href = $"{self}/summary", Method = "GET" }
        };
    }

    public static List<LinkDto> ForShopSummary(Guid shopId)
    {
        var shop = $"/api/shops/{shopId}";

        return new List<LinkDto>
        {
            new LinkDto { Rel = "self", Href = $"{shop}/summary", Method = "GET" },
            new LinkDto { Rel = "shop", Href = shop, Method = "GET" },
            new LinkDto { Rel = "products", Href = $"{shop}/products", Method = "GET" }
        };
    }

    public static List<LinkDto> ForProduct(Guid shopId, Guid productId)
    {
        var shop = $"/api/shops/{shopId}";
        var self = $"{shop}/products/{productId}";

        return new List<LinkDto>
        {
            new LinkDto { Rel = "self", Href = self, Method = "GET" },
            new LinkDto { Rel = "update", Href = self, Method = "PUT" },
            new LinkDto { Rel = "delete", Href = self, Method = "DELETE" },
            new LinkDto { Rel = "shop", Href = shop, Method = "GET" },
            new LinkDto { Rel = "reviews", Href = $"{self}/reviews", Method = "GET" }
        };
    }

    public static List<LinkDto> ForReview(Guid shopId, Guid productId, Guid reviewId)
    {
        var product = $"/api/shops/{shopId}/products/{productId}";
        var self = $"{product}/reviews/{reviewId}";

        return new List<LinkDto>
        {
            new LinkDto { Rel = "self", Href = self, Method = "GET" },
            new LinkDto { Rel = "update", Href = self, Method = "PUT" },
            new LinkDto { Rel = "delete", Href = self, Method = "DELETE" },
            new LinkDto { Rel = "product", Href = product, Method = "GET" }
        };
    }

    public static List<LinkDto> ForPage(HttpRequest request, int page, int totalPages)
    {
        var links = new List<LinkDto>();

        links.Add(new LinkDto { Rel = "self", Href = PageHref(request, page), Method = "GET" });
        links.Add(new LinkDto { Rel = "first", Href = PageHref(request, 1), Method = "GET" });

        if (page > 1)
        {
            links.Add(new LinkDto { Rel = "prev", Href = PageHref(request, page - 1), Method = "GET" });
        }

        if (page < totalPages)
        {
            links.Add(new LinkDto { Rel = "next", Href = PageHref(request, page + 1), Method = "GET" });
        }

        if (totalPages > 0)
        {
            links.Add(new LinkDto { Rel = "last", Href = PageHref(request, totalPages), Method = "GET" });
        }

        return links;
    }

    private static string PageHref(HttpRequest request, int page)
    {
        var href = request.Path + "?page=" + page;

        foreach (var parameter in request.Query)
        {
            if (parameter.Key.ToLower() != "page")
            {
                href += "&" + parameter.Key + "=" + Uri.EscapeDataString(parameter.Value.ToString());
            }
        }

        return href;
    }
}