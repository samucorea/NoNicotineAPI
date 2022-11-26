using Microsoft.EntityFrameworkCore;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly AppDbContext _context;
        public EntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEntryAsync(Entry entry, CancellationToken cancellationToken)
        { 

            await _context.Entry.AddAsync(entry, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken) == 1;
        }

        public async Task<Entry?> GetPatientEntryByIdAsync(string patiendId, string entryId, CancellationToken cancellationToken)
        {
            var entry = await _context.Entry.Where(entry => entry.ID == entryId && entry.PatientId == patiendId).FirstOrDefaultAsync(cancellationToken);
            if (entry == null)
            {
                return null;
            }

            return entry;
        }
        public async Task<List<Entry>> GetPatientEntriesAsync(string patiendId, CancellationToken cancellationToken)
        {
            var entries = await _context.Entry.Where(entry => entry.PatientId == patiendId).ToListAsync(cancellationToken);

            return entries;
        }
        public async Task<List<Entry>?> GetPatientSharedEntriesAsync(string patiendId, CancellationToken cancellationToken)
        {
            var entries = await _context.Entry.Where(entry => entry.PatientId == patiendId && entry.TherapistAllowed == true).ToListAsync(cancellationToken);
            if (entries.Count < 1)
            {
                return null;
            }

            return entries;
        }
    }
}
