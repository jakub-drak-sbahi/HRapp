﻿// <auto-generated />
using System;
using MainApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MainApp.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MainApp.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailAddress");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("MainApp.Models.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CandidateId");

                    b.Property<bool>("ContactAgreement");

                    b.Property<string>("CvUrl");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int?>("JobOfferId");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("State");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("JobOfferId");

                    b.ToTable("JobApplications");

                    b.HasData(
                        new { Id = 1, CandidateId = 7, ContactAgreement = true, CvUrl = "www.google.com", EmailAddress = "otylypan@donna.com", FirstName = "Bartosz", JobOfferId = 4, LastName = "Walaszek", PhoneNumber = "2377746467", State = "Pending" },
                        new { Id = 2, CandidateId = 2, ContactAgreement = true, CvUrl = "www.google.com", EmailAddress = "deus@vultInfidels.com", FirstName = "Ryszard", JobOfferId = 2, LastName = "Lwie Serce", PhoneNumber = "435777654", State = "Pending" },
                        new { Id = 3, CandidateId = 9, ContactAgreement = true, CvUrl = "www.google.com", EmailAddress = "zlazoj@spoko.pl", FirstName = "Slavoj", JobOfferId = 6, LastName = "Zizek", PhoneNumber = "23456764", State = "Pending" },
                        new { Id = 4, CandidateId = 9, ContactAgreement = true, CvUrl = "www.google.com", EmailAddress = "zlazoj@spoko.pl", FirstName = "Slavoj", JobOfferId = 7, LastName = "Zizek", PhoneNumber = "23456764", State = "Pending" }
                    );
                });

            modelBuilder.Entity("MainApp.Models.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Candidates");

                    b.HasData(
                        new { Id = 1, EmailAddress = "beans@yahoo.com", FirstName = "Johny", LastName = "English", PhoneNumber = "889273765" },
                        new { Id = 2, EmailAddress = "deus@vult.com", FirstName = "Ryszard", LastName = "Lwie Serce", PhoneNumber = "435777654" },
                        new { Id = 3, EmailAddress = "hannibal@mini.pw.edu.pl", FirstName = "Hannibal", LastName = "Lecter", PhoneNumber = "566433567" },
                        new { Id = 4, EmailAddress = "jackkk@yahoo.com", FirstName = "Jack", LastName = "Nicolson", PhoneNumber = "567372372" },
                        new { Id = 5, EmailAddress = "priviet@yahoo.com", FirstName = "Lyudmiła", LastName = "Pawliczenko", PhoneNumber = "678363862" },
                        new { Id = 6, EmailAddress = "phrl@yahoo.com", FirstName = "Pharrel", LastName = "Williams", PhoneNumber = "6472638263" },
                        new { Id = 7, EmailAddress = "otylypan@donna.com", FirstName = "Bartosz", LastName = "Walaszek", PhoneNumber = "2377746467" },
                        new { Id = 8, EmailAddress = "batman@gotham.com", FirstName = "Bruce", LastName = "Wayne", PhoneNumber = "367473783" },
                        new { Id = 9, EmailAddress = "zlazoj@spoko.pl", FirstName = "Slavoj", LastName = "Zizek", PhoneNumber = "74638368" }
                    );
                });

            modelBuilder.Entity("MainApp.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApplicationId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Comments");

                    b.HasData(
                        new { Id = 1, ApplicationId = 3, Text = "Well well well" },
                        new { Id = 2, ApplicationId = 4, Text = "Well well" }
                    );
                });

            modelBuilder.Entity("MainApp.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new { Id = 1, Name = "Abibas" },
                        new { Id = 2, Name = "Niky" },
                        new { Id = 3, Name = "Binbows" },
                        new { Id = 4, Name = "Facefood" },
                        new { Id = 5, Name = "Sunbucks coffee" },
                        new { Id = 6, Name = "Polystation" },
                        new { Id = 7, Name = "Dolce&Banana" },
                        new { Id = 8, Name = "KFG" },
                        new { Id = 9, Name = "Hike" },
                        new { Id = 10, Name = "Drunkin nonuts" }
                    );
                });

            modelBuilder.Entity("MainApp.Models.HR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("HRs");

                    b.HasData(
                        new { Id = 1, CompanyId = 9, EmailAddress = "bogusia67@wp.pl", FirstName = "Bogumiła", LastName = "Kowalska" },
                        new { Id = 2, CompanyId = 10, EmailAddress = "grazyna.nowak@autograf.pl", FirstName = "Grażyna", LastName = "Nowak" },
                        new { Id = 3, CompanyId = 10, EmailAddress = "karennn@onet.pl", FirstName = "Karen", LastName = "Kurka" },
                        new { Id = 4, CompanyId = 7, EmailAddress = "zofia_skorupka@gmail.com", FirstName = "Zofia", LastName = "Skorupka" },
                        new { Id = 5, CompanyId = 5, EmailAddress = "anna.wanna@gmail.com", FirstName = "Anna", LastName = "Wanna" },
                        new { Id = 6, CompanyId = 3, EmailAddress = "janette.podsiadlo@gmail.com", FirstName = "Żaneta", LastName = "Podsiadło" },
                        new { Id = 7, CompanyId = 1, EmailAddress = "katarzyna.testowa@spoko.pl", FirstName = "Katarzyna", LastName = "Testowa" }
                    );
                });

            modelBuilder.Entity("MainApp.Models.JobOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("HRId");

                    b.Property<string>("JobTitle")
                        .IsRequired();

                    b.Property<string>("Location");

                    b.Property<decimal?>("SalaryFrom");

                    b.Property<decimal?>("SalaryTo");

                    b.Property<DateTime?>("ValidUntil");

                    b.HasKey("Id");

                    b.HasIndex("HRId");

                    b.ToTable("JobOffers");

                    b.HasData(
                        new { Id = 1, Created = new DateTime(2020, 1, 6, 13, 14, 13, 3, DateTimeKind.Local), Description = "Pinnace Brethren of the Coast heave to jury mast bring a spring upon her cable mizzenmastbilge bilge rat chandler crow's nest. Cackle fruit long clothes chantey rigging topsail brig Barbary Coast long boat topmast Sea Legs. Trysail Admiral of the Black pirate jury mast draught mizzenmast execution dock mizzen no prey, no pay yawl.", HRId = 5, JobTitle = "Package manager" },
                        new { Id = 2, Created = new DateTime(2020, 1, 6, 13, 14, 13, 5, DateTimeKind.Local), Description = "Belay lookout chase guns carouser draught scurvy barque haul wind strike colors weigh anchor.Walk the plank Spanish Main aye knave yo - ho - ho Cat o'nine tails furl warp hang the jib grapple. Sheet blow the man down belay gally driver Shiver me timbers jolly boat fluke loot cog.", HRId = 6, JobTitle = "Pirate recruiter" },
                        new { Id = 3, Created = new DateTime(2020, 1, 6, 13, 14, 13, 5, DateTimeKind.Local), Description = "Pinnace wench Buccaneer chase furl chase guns heave to nipper clap of thunder tackle. Gally splice the main brace execution dock Privateer ahoy stern no prey, no pay quarterdeck bowsprit scourge of the seven seas. Tender scuttle Chain Shot stern blow the man down bucko bowsprit to go on account walk the plank flogging.", HRId = 4, JobTitle = "Younger assisstant" },
                        new { Id = 4, Created = new DateTime(2020, 1, 6, 13, 14, 13, 5, DateTimeKind.Local), Description = "Scuttle clap of thunder ho salmagundi six pounders grog blossom cutlass red ensign ballast wherry. Letter of Marque Arr ye come about chase guns code of conduct scuttle jury mast handsomely gabion. Main sheet heave down lookout parrel hornswaggle coxswain handsomely six pounders clap of thunder Chain Shot.", HRId = 5, JobTitle = "Younger assassin" },
                        new { Id = 5, Created = new DateTime(2020, 1, 6, 13, 14, 13, 5, DateTimeKind.Local), Description = "Hempen halter boom bounty hornswaggle fore ballast Sink me hearties ye blow the man down. League topsail Blimey trysail yo-ho-ho rutters yawl scuttle dance the hempen jig Brethren of the Coast. Warp measured fer yer chains six pounders rope's end lugger Pieces of Eight killick black spot hempen halter man-of-war.", HRId = 3, JobTitle = "Younger chief executor" },
                        new { Id = 6, Created = new DateTime(2020, 1, 6, 13, 14, 13, 5, DateTimeKind.Local), Description = "Hempen halter boom bounty hornswaggle fore ballast Sink me hearties ye blow the man down. League topsail Blimey trysail yo-ho-ho rutters yawl scuttle dance the hempen jig Brethren of the Coast. Warp measured fer yer chains six pounders rope's end lugger Pieces of Eight killick black spot hempen halter man-of-war.", HRId = 7, JobTitle = "Younger chief executor" },
                        new { Id = 7, Created = new DateTime(2020, 1, 6, 13, 14, 13, 5, DateTimeKind.Local), Description = "Hempen halter boom bounty hornswaggle fore ballast Sink me hearties ye blow the man down. League topsail Blimey trysail yo-ho-ho rutters yawl scuttle dance the hempen jig Brethren of the Coast. Warp measured fer yer chains six pounders rope's end lugger Pieces of Eight killick black spot hempen halter man-of-war.", HRId = 7, JobTitle = "Butterfly Collector" }
                    );
                });

            modelBuilder.Entity("MainApp.Models.Application", b =>
                {
                    b.HasOne("MainApp.Models.Candidate", "Candidate")
                        .WithMany("Applications")
                        .HasForeignKey("CandidateId");

                    b.HasOne("MainApp.Models.JobOffer", "JobOffer")
                        .WithMany("JobApplications")
                        .HasForeignKey("JobOfferId");
                });

            modelBuilder.Entity("MainApp.Models.Comment", b =>
                {
                    b.HasOne("MainApp.Models.Application", "Application")
                        .WithMany("Comments")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("MainApp.Models.HR", b =>
                {
                    b.HasOne("MainApp.Models.Company", "Company")
                        .WithMany("HRs")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MainApp.Models.JobOffer", b =>
                {
                    b.HasOne("MainApp.Models.HR", "HR")
                        .WithMany("JobOffers")
                        .HasForeignKey("HRId");
                });
#pragma warning restore 612, 618
        }
    }
}
