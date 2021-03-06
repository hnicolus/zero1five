using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Zero1Five.Products;

namespace Zero1Five.Gigs
{
    public class GigDto : FullAuditedEntityDto<Guid>
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }

        public string CategoryName { get; set; }

        public string CoverImage { get; set; }

        public string Description { get; set; }

        public float Rating { get; set; }

        public ICollection<ProductDto> Products { get; set; }

        public bool IsPublished { get; set; }

        public string  LoadCover()  => StorageConsts.GigContainer +CoverImage; 
    }
}