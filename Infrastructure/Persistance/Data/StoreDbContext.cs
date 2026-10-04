using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceEntity = Domain.Models.Service;

namespace Persistance.Data;

// Deriving from IdentityDbContext is the only change Identity needs from this context: it adds the
// AspNetUsers/AspNetUserClaims/... tables and maps ApplicationUser onto them. Every portfolio
// relationship below is untouched.
public class StoreDbContext : IdentityDbContext<ApplicationUser>
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectImage> ProjectImages { get; set; }
    public DbSet<Project_Tag> Project_Tags { get; set; }
    public DbSet<Project_Category> Project_Categories { get; set; }
    public DbSet<Project_Technology> Project_Technologies { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Skill_Type> Skill_Types { get; set; }
    public DbSet<Domain.Models.Type> Types { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<SocialLink> SocialLinks { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Project <Has> ProjectImage
        modelBuilder.Entity<ProjectImage>()
            .HasOne(pi => pi.Project)
            .WithMany(p => p.ProjectImages)
            .HasForeignKey(pi => pi.ProjectId);

        // Project <Has> Project_Tag
        modelBuilder.Entity<Project_Tag>()
            .HasKey(pt => new { pt.ProjectId, pt.TagId });

        modelBuilder.Entity<Project_Tag>()
            .HasOne(pt => pt.Project)
            .WithMany(p => p.Project_Tags)
            .HasForeignKey(pt => pt.ProjectId);

        // Tag <Has> Project_Tag
        modelBuilder.Entity<Project_Tag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.Project_Tags)
            .HasForeignKey(pt => pt.TagId);

        // Project <Has> Project_Category
        modelBuilder.Entity<Project_Category>()
            .HasKey(pc => new { pc.ProjectId, pc.CategoryId });

        modelBuilder.Entity<Project_Category>()
            .HasOne(pc => pc.Project)
            .WithMany(p => p.Project_Categories)
            .HasForeignKey(pc => pc.ProjectId);

        // Category <Has> Project_Category
        modelBuilder.Entity<Project_Category>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.Project_Categories)
            .HasForeignKey(pc => pc.CategoryId);

        // Project <Has> Project_Technology
        modelBuilder.Entity<Project_Technology>()
            .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

        modelBuilder.Entity<Project_Technology>()
            .HasOne(pt => pt.Project)
            .WithMany(p => p.Project_Technologies)
            .HasForeignKey(pt => pt.ProjectId);

        // Technology <Has> Project_Technology
        modelBuilder.Entity<Project_Technology>()
            .HasOne(pt => pt.Technology)
            .WithMany(t => t.Project_Technologies)
            .HasForeignKey(pt => pt.TechnologyId);

        // Skill <Has> Skill_Type
        modelBuilder.Entity<Skill_Type>()
            .HasKey(st => new { st.SkillId, st.TypeId });

        modelBuilder.Entity<Skill_Type>()
            .HasOne(st => st.Skill)
            .WithMany(s => s.Skill_Types)
            .HasForeignKey(st => st.SkillId);

        // Type <Has> Skill_Type
        modelBuilder.Entity<Skill_Type>()
            .HasOne(st => st.Type)
            .WithMany(t => t.Skill_Types)
            .HasForeignKey(st => st.TypeId); 
    }
}
