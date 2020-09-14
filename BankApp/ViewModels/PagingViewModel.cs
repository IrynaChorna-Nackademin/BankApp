using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.ViewModels
{
    public class PagingViewModel
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public double PageCount { get; set; }
        public int MaxPages { get; set; }

        public bool ShowPrevButton => CurrentPage > 1;
        public bool ShowNextButton => CurrentPage < MaxPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != MaxPages;

        public IEnumerable<SelectListItem> PageSizeOptions => new[] {
            new SelectListItem("10", "10"),
            new SelectListItem("20", "20"),
            new SelectListItem("50", "50"),
            new SelectListItem("100", "100")
        };
        public IEnumerable<string> GetPages
        {
            get
            {
                int delta = 2;
                int left = CurrentPage - delta;
                int right = CurrentPage + delta + 1;

                var range = new List<string>();
                for (int i = 1; i <= MaxPages; i++)
                    if (i == 1 || i == MaxPages || (i >= left && i < right))
                        range.Add(i.ToString());

                var rangeIncludingDots = new List<string>();
                int l = 0;
                foreach (var i in range.Select(r => Convert.ToInt32(r)))
                {
                    if (l > 0)
                    {
                        if (i - l == 2)
                            rangeIncludingDots.Add((l + 1).ToString());
                        else if (i - l != 1)
                            rangeIncludingDots.Add("...");
                    }

                    rangeIncludingDots.Add(i.ToString());
                    l = i;
                }

                return rangeIncludingDots;
            }
        }
    }
}