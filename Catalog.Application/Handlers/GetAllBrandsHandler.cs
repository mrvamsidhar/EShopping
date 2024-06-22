using AutoMapper;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers;

public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
{
    public readonly IBrandRepository _brandRepository;    
    public GetAllBrandsHandler(IBrandRepository repository) 
    { 
        _brandRepository = repository;        
    }

    public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brandList = await _brandRepository.GetAllBrands();
        var response = ProductMapper.Mapper.Map<IList<ProductBrand>, IList<BrandResponse>>(brandList.ToList());
        return response;
    }
}
