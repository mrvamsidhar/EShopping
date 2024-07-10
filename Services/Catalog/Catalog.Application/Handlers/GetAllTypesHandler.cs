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
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        public readonly ITypesRepository _repository;
        public GetAllTypesHandler(ITypesRepository repository)
        {
            _repository = repository;
        }
        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var productList = await _repository.GetAllTypes();
            var response = ProductMapper.Mapper.Map<IList<TypesResponse>>(productList);
            return response;
        }
    }
}
