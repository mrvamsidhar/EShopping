﻿using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mappers;

public class ProductMappingProfile: Profile
{
    public ProductMappingProfile() { 
        CreateMap<ProductBrand, BrandResponse>();
        CreateMap<ProductType, TypesResponse>();
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, CreateProductCommand>().ReverseMap();
        CreateMap<Pagination<Product>, Pagination<ProductResponse>>().ReverseMap();
    }
}
