using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Domain.Repository;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.CastMember.CreateCastMember
{
	public class CreateCastMember : ICreateCastMember
	{
		private readonly ICastMemberRepository _repository;
		private readonly IUnitOfWork _unitOfWork;
		
		public async Task<CastMemberModelOutput> Handle(CreateCastMemberInput request, CancellationToken cancellationToken)
		{
			var castMember = new DomainEntity.CastMember(request.Name, request.Type);
			await _repository.Insert(castMember, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return CastMemberModelOutput.FromCastMember(castMember);
		}


		public CreateCastMember(ICastMemberRepository repository, IUnitOfWork unitOfWork)
			=> (_repository, _unitOfWork) = (repository, unitOfWork);
	}
}
