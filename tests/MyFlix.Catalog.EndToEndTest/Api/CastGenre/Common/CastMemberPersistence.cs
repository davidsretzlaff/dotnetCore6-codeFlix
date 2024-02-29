using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Infra.Data.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.EndToEndTest.Api.CastGenre.Common
{
	public class CastMemberPersistence
	{
		private readonly CatalogDbContext _context;

		public CastMemberPersistence(CatalogDbContext context)
			=> _context = context;

		public async Task InsertList(List<DomainEntity.CastMember> castMember)
		{
			await _context.AddRangeAsync(castMember);
			await _context.SaveChangesAsync();
		}

		public async Task<DomainEntity.CastMember?> GetById(Guid id)
			=> await _context.CastMembers.AsNoTracking().FirstOrDefaultAsync(castMember => castMember.Id == id);

	}
}
