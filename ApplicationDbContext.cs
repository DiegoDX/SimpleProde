using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleProde.Entities;
using System.Reflection.Emit;

namespace SimpleProde
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>(ConfigureTeam);
            modelBuilder.Entity<Championship>(ConfigureChampionship);
            modelBuilder.Entity<Score>(ConfigureScore);
            modelBuilder.Entity<Match>(ConfigureMatch);
            modelBuilder.Entity<Bet>(ConfigureBet);



            //modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
            //modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            //modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims");
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogins");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles");
            //modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosTokens");

        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Championship> Championships { get; set; }
        public DbSet<Match> Matchs { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<User> Users { get; set; }

        private void ConfigureTeam(EntityTypeBuilder<Team> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
            builder.Property(p => p.ShirtColor).IsUnicode();
            builder.Property(p => p.Flag).IsUnicode();


            builder.HasMany(t => t.HomeMatches)
            .WithOne(m => m.HomeTeam)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureChampionship(EntityTypeBuilder<Championship> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Icon).IsUnicode();
        }

        private void ConfigureScore(EntityTypeBuilder<Score> builder)
        {
            builder.HasOne(b => b.User)
               .WithMany(u => u.Scores)
               .HasForeignKey(b => b.UserId)
               .IsRequired();

            builder.HasOne(b => b.Championship)
              .WithMany(u => u.Scores)
              .HasForeignKey(b => b.ChampionshipId)
              .IsRequired();
        }

        private void ConfigureMatch(EntityTypeBuilder<Match> builder)
        {
            builder
               .HasOne(m => m.Championship)
               .WithMany(c => c.Matches)
               .HasForeignKey(m => m.ChampionshipId)
               .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(m => m.HomeTeam)
               .WithMany(t => t.HomeMatches)
               .HasForeignKey(m => m.HomeTeamId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureBet(EntityTypeBuilder<Bet> builder)
        {
            builder.Property(p => p.HomeTeamScore).IsRequired();
            builder.Property(p => p.AwayTeamScore).IsRequired();
           // builder.HasKey(g => new { g.UserId, g.MatchId});

            builder.HasOne(b => b.User)
            .WithMany(u => u.Bets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Match)
           .WithMany(u => u.Bets)
           .HasForeignKey(b => b.MatchId)
           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
