// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WisePay.DataAccess;
using WisePay.Entities;

namespace WisePay.DataAccess.Migrations
{
    [DbContext(typeof(WiseContext))]
    [Migration("20171229230915_UpdateUserPurchaseItemBehaviour")]
    partial class UpdateUserPurchaseItemBehaviour
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WisePay.Entities.PaymentHistoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PurchaseId");

                    b.Property<decimal>("Sum");

                    b.Property<DateTime>("Timestamp");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentHistory");
                });

            modelBuilder.Entity("WisePay.Entities.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatorId");

                    b.Property<bool>("IsPayedOff");

                    b.Property<string>("Name");

                    b.Property<decimal?>("TotalSum");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("WisePay.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("WisePay.Entities.StoreOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsSubmitted");

                    b.Property<int>("PurchaseId");

                    b.Property<string>("StoreId");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId")
                        .IsUnique();

                    b.ToTable("StoreOrders");
                });

            modelBuilder.Entity("WisePay.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdminId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("WisePay.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AvatarPath");

                    b.Property<string>("BankActionToken");

                    b.Property<string>("BankIdToken");

                    b.Property<string>("CardLastFourDigits");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WisePay.Entities.UserPurchase", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PurchaseId");

                    b.Property<int>("Status");

                    b.Property<decimal?>("Sum");

                    b.HasKey("UserId", "PurchaseId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("UserPurchases");
                });

            modelBuilder.Entity("WisePay.Entities.UserPurchaseItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ItemId");

                    b.Property<long>("Number");

                    b.Property<decimal>("Price");

                    b.Property<int?>("UserPurchasePurchaseId");

                    b.Property<int?>("UserPurchaseUserId");

                    b.HasKey("Id");

                    b.HasIndex("UserPurchaseUserId", "UserPurchasePurchaseId");

                    b.ToTable("UserPurchaseItems");
                });

            modelBuilder.Entity("WisePay.Entities.UserTeam", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("TeamId");

                    b.HasKey("UserId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("UserTeams");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("WisePay.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("WisePay.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("WisePay.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("WisePay.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WisePay.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("WisePay.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.PaymentHistoryItem", b =>
                {
                    b.HasOne("WisePay.Entities.Purchase", "Purchase")
                        .WithMany()
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WisePay.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.Purchase", b =>
                {
                    b.HasOne("WisePay.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.StoreOrder", b =>
                {
                    b.HasOne("WisePay.Entities.Purchase", "Purchase")
                        .WithOne("StoreOrder")
                        .HasForeignKey("WisePay.Entities.StoreOrder", "PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.Team", b =>
                {
                    b.HasOne("WisePay.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.UserPurchase", b =>
                {
                    b.HasOne("WisePay.Entities.Purchase", "Purchase")
                        .WithMany("UserPurchases")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WisePay.Entities.User", "User")
                        .WithMany("UserPurchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.UserPurchaseItem", b =>
                {
                    b.HasOne("WisePay.Entities.UserPurchase", "UserPurchase")
                        .WithMany("Items")
                        .HasForeignKey("UserPurchaseUserId", "UserPurchasePurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WisePay.Entities.UserTeam", b =>
                {
                    b.HasOne("WisePay.Entities.Team", "Team")
                        .WithMany("UserTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WisePay.Entities.User", "User")
                        .WithMany("UserTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}