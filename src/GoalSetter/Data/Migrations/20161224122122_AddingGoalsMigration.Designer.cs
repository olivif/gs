using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GoalSetter.Data;

namespace GoalSetter.Migrations
{
    [DbContext(typeof(GoalsDbContext))]
    [Migration("20161224122122_AddingGoalsMigration")]
    partial class AddingGoalsMigration
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GoalSetter.Models.Goals.GoalViewModel", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("GoalId");

                    b.Property<string>("Data");

                    b.HasKey("UserId", "GoalId");

                    b.ToTable("Goals");
                });
        }
    }
}
