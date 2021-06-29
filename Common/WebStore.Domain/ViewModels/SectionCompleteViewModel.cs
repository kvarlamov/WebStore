﻿using System.Collections.Generic;

namespace WebStore.Domain.ViewModels
{
    public class SectionCompleteViewModel
    {
        public IEnumerable<SectionViewModel> Sections { get; set; }
        public int? CurrentParentSection { get; set; }
        public int? CurrentSectionId { get; set; }
    }
}