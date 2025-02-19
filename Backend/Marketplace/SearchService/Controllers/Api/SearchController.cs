﻿using Microsoft.AspNetCore.Mvc;
using SearchService.Models.Search;
using SearchService.Services;

namespace SearchService.Controllers.Api
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : Controller
    {
        private FindService _findService;

        public SearchController(FindService findService)
        {
            _findService = findService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search(string q, int from, int to)
        {
            List<SearchedProduct>? products = await _findService.SearchProducts(q);
            if (products == null || products.Count == 0)
                    return NotFound();

            if (to >=  products.Count)
                return Ok(products[from..products.Count]);

            return Ok(products[from..to]);
        }
    }
}
