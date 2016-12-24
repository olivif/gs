﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GoalSetter.Data;

namespace GoalSetter.Migrations
{
    [DbContext(typeof(GoalsDbContext))]
    partial class GoalsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GoalSetter.ModelsLogic.Goal", b =>
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
