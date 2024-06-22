using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        public readonly IProductRepository _repository;
        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProduct(request.Id);
            var productResponse = ProductMapper.Mapper.Map<ProductResponse>(product);
            return productResponse;
        }
    }
}
