using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Zero1Five.Products
{
    public interface IProductAppService : ICrudAppService<
            ProductDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateProductDto,
            UpdateProductDto>
    {

    }
}