﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Archive;
using Models.Messages;
using Models.Navigation;
using Models.TicketArchive;
using Models.Tickets;
using Models.Users;

namespace Database.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<WorkflowHistory> WorkflowHistories { get; set; }
    public DbSet<SolverHistory> SolverHistories { get; set; }
    public DbSet<MessageThread> Threads { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<SerializedNavigation> Navigations { get; set; }
    public DbSet<TicketArchive> TicketArchives { get; set; }
    public DbSet<SolverArchiveHistory> SolverArchives { get; set; }
}