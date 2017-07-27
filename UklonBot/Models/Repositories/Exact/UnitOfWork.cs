using UklonBot.Models.Repositories.Abstract;

namespace UklonBot.Models.Repositories.Exact
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private GenericRepository<User> _usersRepository;
        private GenericRepository<ChannelUser> _channelUsersRepository;
        public void Dispose()
        {
            _context.Dispose();
        }

        public GenericRepository<User> Users => _usersRepository ?? (_usersRepository = new GenericRepository<User>(_context));

        public GenericRepository<ChannelUser> ChannelUsers => _channelUsersRepository ??
                                                              (_channelUsersRepository =
                                                                  new GenericRepository<ChannelUser>(_context));

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}