using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace MyShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HomeBanner>().HasData(
                new HomeBanner()
                {
                    Id = 1,
                    Order = 0,
                },
                new HomeBanner()
                {
                    Id = 2,
                    Order = 1
                },
                new HomeBanner()
                {
                    Id = 3,
                    Order = 2
                },
                new HomeBanner()
                {
                    Id = 4,
                    Order = 3
                },
                new HomeBanner()
                {
                    Id = 5,
                    Order = 4
                }

                );
            modelBuilder.Entity<About>().HasData(
                new About()
                {
                    Id = 1
                });
            modelBuilder.Entity<HomePage>().HasData(
                new HomePage()
                {
                    Id = 1,
                    Has24Support = true,
                    HasFastDeliveryOption = true,
                    HasLocalPaymentOption = true,
                    HasOriginalWarranty = true
                });
            modelBuilder.Entity<ContactUs>().HasData(new ContactUs() {
            Id = 1
            
            });
            modelBuilder.Entity<Term>().HasData(
                new Term() { Id = 1, Content = @"Terms of world wide e-commerce" });
            modelBuilder.Entity<Delivery>().HasData(
                new Delivery()
                {
                    Id = 1,
                    CanSendingToAllCity = true,
                    CityPriceStatus = CityPriceStatus.FreeForAllCities,
                    HasMinAmountForFreeDelivery = false,
                    MinAmountForFreeDelivery = 0,
                   
                }
                );
            modelBuilder.Entity<Notification>().HasData(new Notification() {Id = 1, IsPublished = false });
            modelBuilder.Entity<State>().HasData(
                new State() { Id = 1, DeliveryId = 1,StateName = "BARINGO" },
                new State() { Id = 2, DeliveryId = 1,StateName = "BOMET" },
                new State() { Id = 3, DeliveryId = 1,StateName = "BUNGOMA" },
                new State() { Id = 4, DeliveryId = 1,StateName = "BUSIA" },
                new State() { Id = 5, DeliveryId = 1,StateName = "ELEGEYO-MARAKWET" },
                new State() { Id = 6, DeliveryId = 1,StateName = "EMBU" },
                new State() { Id = 7, DeliveryId = 1,StateName = "GARISSA" },
                new State() { Id = 8, DeliveryId = 1,StateName = "HOMA BAY" },
                new State() { Id = 9, DeliveryId = 1,StateName = "ISIOLO" },
                new State() { Id = 10,DeliveryId = 1, StateName = "KAJIADO" },
                new State() { Id = 11,DeliveryId = 1, StateName = "KAKAMEGA" },
                new State() { Id = 12,DeliveryId = 1, StateName = "KERICHO" },
                new State() { Id = 13,DeliveryId = 1, StateName = "KIAMBU" },
                new State() { Id = 14,DeliveryId = 1, StateName = "KILIFI" },
                new State() { Id = 15,DeliveryId = 1, StateName = "KIRINYAGA" },
                new State() { Id = 16,DeliveryId = 1, StateName = "KISII" },
                new State() { Id = 17,DeliveryId = 1, StateName = "KISUMU" },
                new State() { Id = 18,DeliveryId = 1, StateName = "KITUI" },
                new State() { Id = 19,DeliveryId = 1, StateName = "KWALE" },
                new State() { Id = 20,DeliveryId = 1, StateName = "LAIKIPIA" },
                new State() { Id = 21,DeliveryId = 1, StateName = "LAMU" },
                new State() { Id = 22,DeliveryId = 1, StateName = "MACHAKOS" },
                new State() { Id = 23,DeliveryId = 1, StateName = "MAKUENI" },
                new State() { Id = 24,DeliveryId = 1, StateName = "MANDERA" },
                new State() { Id = 25,DeliveryId = 1, StateName = "MARSABIT" },
                new State() { Id = 26,DeliveryId = 1, StateName = "MERU" },
                new State() { Id = 27,DeliveryId = 1, StateName = "MIGORI" },
                new State() { Id = 28,DeliveryId = 1, StateName = "MOMBASA" },
                new State() { Id = 29,DeliveryId = 1, StateName = "MURANG'A" },
                new State() { Id = 30,DeliveryId = 1, StateName = "NAIROBI" },
                new State() { Id = 31,DeliveryId = 1, StateName = "NAKURU" },
                new State() { Id = 32,DeliveryId = 1, StateName = "NANDI" },
                new State() { Id = 33,DeliveryId = 1, StateName = "NAROK" },
                new State() { Id = 34,DeliveryId = 1, StateName = "NYAMIRA" },
                new State() { Id = 35,DeliveryId = 1, StateName = "NYANDARUA" },
                new State() { Id = 36,DeliveryId = 1, StateName = "NYERI" },
                new State() { Id = 37,DeliveryId = 1, StateName = "SAMBURU" },
                new State() { Id = 38,DeliveryId = 1, StateName = "SIAYA" },
                new State() { Id = 39,DeliveryId = 1, StateName = "TITA TAVETA" },
                new State() { Id = 40,DeliveryId = 1, StateName = "TANA RIVER" },
                new State() { Id = 41,DeliveryId = 1, StateName = "THARAKA-NTHI" },
                new State() { Id = 42,DeliveryId = 1, StateName = "TRANS ANZOIA" },
                new State() { Id = 43,DeliveryId = 1, StateName = "TURKANA" },
                new State() { Id = 44,DeliveryId = 1, StateName = "UASIN GISHU" },
                new State() { Id = 45,DeliveryId = 1, StateName = "VIHIGA" },
                new State() { Id = 46,DeliveryId = 1, StateName = "WAJIR" },
                new State() { Id = 47,DeliveryId = 1, StateName = "WEST POKOT" });

            modelBuilder.Entity<City>()
                .HasData(
                //BARINGO Cities
                new City() { Id = 1, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BARINGO", StateId = 1, Title = "TIATY" },
                new City() { Id = 2, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BARINGO", StateId = 1, Title = "BARINGO NORTH" },
                new City() { Id = 3, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BARINGO", StateId = 1, Title = "BARINGO CENTRAL" },
                new City() { Id = 4, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BARINGO", StateId = 1, Title = "BARINGO SOUTH" },
                new City() { Id = 5, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BARINGO", StateId = 1, Title = "MOGOTIO" },
                new City() { Id = 6, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BARINGO", StateId = 1, Title = "ELDAMA RAVINE" },
            

                //BOMET Cities
                new City() { Id = 7, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BOMET", StateId = 2, Title = "SOTIK" },
                new City() { Id = 8, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BOMET", StateId = 2, Title = "CHEPALUNGU" },
                new City() { Id = 9, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BOMET", StateId = 2, Title = "BOMET EAST" },
                new City() { Id = 10, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BOMET", StateId = 2, Title = "BOMET CENTRAL" },
                new City() { Id = 11, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BOMET", StateId = 2, Title = "KONOIN" },
              
             
                //BUNGOMA Cities
                new City() { Id = 12, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "MT. ELGON" },
                new City() { Id = 13, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "SIRISIA" },
                new City() { Id = 14, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "KABUCHAI" },
                new City() { Id = 15, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "BUMULA" },
                new City() { Id = 16, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "KANDUYI" },
                new City() { Id = 17, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "WEBUYE EAST" },
                new City() { Id = 18, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "WEBUYE WEST" },
                new City() { Id = 19, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "KIMILILI" },
                new City() { Id = 20, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUNGOMA", StateId = 3, Title = "TONGAREN" },

                  //BUSIA Cities
                  new City() { Id = 21, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "TESO NORTH" },
                  new City() { Id = 22, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "TESO SOUTH" },
                  new City() { Id = 23, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "NAMBALE" },
                  new City() { Id = 24, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "MATAYOS" },
                  new City() { Id = 25, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "BUTULA" },
                  new City() { Id = 26, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "FUNYULA" },
                  new City() { Id = 27, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "BUSIA", StateId = 4, Title = "BUDALANGI" },


                   //ELEGEYO-MARKAWET Cities
                   new City() { Id = 28, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "ELEGEYO-MARKAWET", StateId = 5, Title = "MARKAWET EAST" },
                   new City() { Id = 29, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "ELEGEYO-MARKAWET", StateId = 5, Title = "KEIYO SOUTH" },
                   new City() { Id = 30, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "ELEGEYO-MARKAWET", StateId = 5, Title = "KEIYO NORTH" },


                    //EMBU Cities
                    new City() { Id = 31, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "EMBU", StateId = 6, Title = "MANYATTA" },
                    new City() { Id = 32, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "EMBU", StateId = 6, Title = "RUNYENJES" },
                    new City() { Id = 33, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "EMBU", StateId = 6, Title = "MBEERE SOUTH" },
                    new City() { Id = 34, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "EMBU", StateId = 6, Title = "MBEERE NORTH" },


                    //GARISSA Cities
                    new City() { Id = 35, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "GARISSA", StateId = 7, Title = "GARISSA TOWNSHIP" },
                    new City() { Id = 36, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "GARISSA", StateId = 7, Title = "BALAMBALA" },
                    new City() { Id = 37, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "GARISSA", StateId = 7, Title = "LAGDERA" },
                    new City() { Id = 38, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "GARISSA", StateId = 7, Title = "DADAAB" },
                    new City() { Id = 39, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "GARISSA", StateId = 7, Title = "FAFI" },
                    new City() { Id = 40, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "GARISSA", StateId = 7, Title = "IJARA" },



                    //HOME BAY Cities
                    new City() { Id = 41, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "HOME BAY", StateId = 8, Title = "KASIPUL" },
                    new City() { Id = 42, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "HOME BAY", StateId = 8, Title = "KABONDO KASIPUL" },
                    new City() { Id = 43, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName ="HOME BAY", StateId = 8, Title = "KARACHUNOYO" },
                    new City() { Id = 44, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName ="HOME BAY", StateId = 8, Title = "RANGWE" },
                    new City() { Id = 45, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName ="HOME BAY", StateId = 8, Title = "HOMA BAY TOWN" },
                    new City() { Id = 46, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName ="HOME BAY", StateId = 8, Title = "NDHIWA" },
                    new City() { Id = 47, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName ="HOME BAY", StateId = 8, Title = "MBITA" },
                    new City() { Id = 48, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "HOME BAY", StateId = 8, Title = "SUBA" },


                    //ISIOLO Cities
                    new City() { Id = 49, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "ISIOLO", StateId = 9, Title = "ISIOLO NORHT" },
                    new City() { Id = 50, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "ISIOLO", StateId = 9, Title = "ISIOLO SOUTH" },


                    //KAJIADO Cities
                    new City() { Id = 51, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAJIADO", StateId = 10, Title = "KAJIADO NORTH" },
                    new City() { Id = 52, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAJIADO", StateId = 10, Title = "KAJIADO CENTRAL" },
                    new City() { Id = 53, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAJIADO", StateId = 10, Title = "KAJIADO EAST" },
                    new City() { Id = 54, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAJIADO", StateId = 10, Title = "KAJIADO WEST" },
                    new City() { Id = 55, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAJIADO", StateId = 10, Title = "KAJIADO SOUTH" },
                    
                    //KAKAMEGA Cities
                    new City() { Id = 56, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "LUGARI" },
                    new City() { Id = 57, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "LIKUYANI" },
                    new City() { Id = 58, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "MALAVA" },
                    new City() { Id = 59, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "LURAMBI" },
                    new City() { Id = 60, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "NAVAKHOLO" },
                    new City() { Id = 61, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "MUMIAS WEST" },
                    new City() { Id = 62, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "MUMIAS EAST" },
                    new City() { Id = 63, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "MATUNGU" },
                    new City() { Id = 64, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "BUTERE" },
                    new City() { Id = 65, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "KHWISERO" },
                    new City() { Id = 66, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "SHINYALU" },
                    new City() { Id = 67, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KAKAMEGA", StateId = 11, Title = "IKOLOMANI" },


                    //KERICHO Cities
                    new City() { Id = 68, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KERICHO", StateId = 12, Title = "KIPKELION EAST" },
                    new City() { Id = 69, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KERICHO", StateId = 12, Title = "KIPKELION WEST" },
                    new City() { Id = 70, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KERICHO", StateId = 12, Title = "AINAMOI" },
                    new City() { Id = 71, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KERICHO", StateId = 12, Title = "BURETI" },
                    new City() { Id = 72, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KERICHO", StateId = 12, Title = "BELGUT" },
                    new City() { Id = 73, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KERICHO", StateId = 12, Title = "SIGOWET/SOIN" },


                    //KIAMBU Cities
                    new City() { Id = 74, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "GATUNDU SOUTH" },
                    new City() { Id = 75, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "GATUNDU NORTH" },
                    new City() { Id = 76, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "JUJA" },
                    new City() { Id = 77, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "THIKA TOWN" },
                    new City() { Id = 78, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "RUIRU" },
                    new City() { Id = 79, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "GITHUNGURI" },
                    new City() { Id = 80, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "KIAMBU" },
                    new City() { Id = 81, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "KIAMBAA" },
                    new City() { Id = 82, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "KABETE" },
                    new City() { Id = 83, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "KIKUYU" },
                    new City() { Id = 84, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "LIMURU" },
                    new City() { Id = 85, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIAMBU", StateId = 13, Title = "LARI" },
                   

                    //KILIFI Cities
                    new City() { Id = 86, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "KILIFI NORTH" },
                    new City() { Id = 87, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "KILIFI SOUTH" },
                    new City() { Id = 88, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "KALOLENI" },
                    new City() { Id = 89, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "RABAI" },
                    new City() { Id = 90, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "GANZE" },
                    new City() { Id = 91, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "MALINDI" },
                    new City() { Id = 92, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KILIFI", StateId = 14, Title = "MAGARINI" },
                   

                    //KIRINYAGA Cities
                    new City() { Id = 93, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIRINYAGA", StateId = 15, Title = "MWEA" },
                    new City() { Id = 94, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIRINYAGA", StateId = 15, Title = "GICHUGU" },
                    new City() { Id = 95, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIRINYAGA", StateId = 15, Title = "NDIA" },
                    new City() { Id = 96, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KIRINYAGA", StateId = 15, Title = "KIRINYAGA CENTRAL" },
                   

                    //KISII Cities
                    new City() { Id = 97, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "BONCHARI" },
                    new City() { Id = 98, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "SOUTH MUGIRANGO" },
                    new City() { Id = 99, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "BOMACHOGE BORABU" },
                    new City() { Id = 100, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "BOBASI" },
                    new City() { Id = 101, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "BOMACHOGE CHACHE" },
                    new City() { Id = 102, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "NYARIBARI MASABA" },
                    new City() { Id = 103, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "NYARIBARI CHACHE" },
                    new City() { Id = 104, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "KITUTU CHACHE NORTH" },
                    new City() { Id = 105, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISII", StateId = 16, Title = "KITUTU CHACHE SOUTH" },

                    //KISUMU Cities
                    new City() { Id = 106, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "KISUMU EAST" },
                    new City() { Id = 107, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "KISUMU WEST" },
                    new City() { Id = 108, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "KISUMU CENTRAL" },
                    new City() { Id = 109, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "SEME" },
                    new City() { Id = 110, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "NYANDO" },
                    new City() { Id = 111, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "MUHORONI" },
                    new City() { Id = 112, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KISUMU", StateId = 17, Title = "NYAKACH" },
                   

                    //KITUI Cities
                    new City() { Id = 113, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "MWINGI NORTH" },
                    new City() { Id = 114, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "MWINGI WEST" },
                    new City() { Id = 115, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "MWINGI CENTRAL" },
                    new City() { Id = 116, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "KITUI WEST" },
                    new City() { Id = 117, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "KITUI RURAL" },
                    new City() { Id = 118, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "KITUI CENTRAL" },
                    new City() { Id = 119, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "KITUI EAST" },
                    new City() { Id = 120, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KITUI", StateId = 18, Title = "KITUI SOUTH" },

                    //KWALE Cities
                    new City() { Id = 121, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KWALE", StateId = 19, Title = "MSAMBWENI" },
                    new City() { Id = 122, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KWALE", StateId = 19, Title = "LUNGA LUNGA" },
                    new City() { Id = 123, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KWALE", StateId = 19, Title = "MATUGA" },
                    new City() { Id = 124, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "KWALE", StateId = 19, Title = "KINANGO" },

                    //LAIKIPIA Cities
                    new City() { Id = 125, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "LAIKIPIA", StateId = 20, Title = "LAIKIPIA WEST" },
                    new City() { Id = 126, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "LAIKIPIA", StateId = 20, Title = "LAIKIPIA EAST" },
                    new City() { Id = 127, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "LAIKIPIA", StateId = 20, Title = "LAIKIPIA NORTH" },
                   

                    //LAMU Cities
                    new City() { Id = 128, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "LAMU", StateId = 21, Title = "LAMU EAST" },
                    new City() { Id = 129, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "LAMU", StateId = 21, Title = "LAMU WEST" },
                  

                    //MACHAKOS Cities
                    new City() { Id = 130, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "MASINGA" },
                    new City() { Id = 131, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "YATTA" },
                    new City() { Id = 132, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "KANGUNDO" },
                    new City() { Id = 133, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "MATUNGULU" },
                    new City() { Id = 134, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "KATHIANI" },
                    new City() { Id = 135, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "MAVOKO" },
                    new City() { Id = 136, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "MACHAKOS TOWN" },
                    new City() { Id = 137, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MACHAKOS", StateId = 22, Title = "MWALA" },
                   

                    //MAKUENI Cities
                    new City() { Id = 138, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MAKUENI", StateId = 23, Title = "MBOONI" },
                    new City() { Id = 139, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MAKUENI", StateId = 23, Title = "KILOME" },
                    new City() { Id = 140, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MAKUENI", StateId = 23, Title = "KAITI" },
                    new City() { Id = 141, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MAKUENI", StateId = 23, Title = "MAKUENI" },
                    new City() { Id = 142, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MAKUENI", StateId = 23, Title = "KIBWEZI WEST" },
                    new City() { Id = 143, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MAKUENI", StateId = 23, Title = "KIBWEZI EAST" },
                   

                    //MANDERA Cities
                    new City() { Id = 144, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MANDERA", StateId = 24, Title = "MANDERA WEST" },
                    new City() { Id = 145, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MANDERA", StateId = 24, Title = "BANISSA" },
                    new City() { Id = 146, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MANDERA", StateId = 24, Title = "MANDERA NORTH" },
                    new City() { Id = 147, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MANDERA", StateId = 24, Title = "MANDERA SOUTH" },
                    new City() { Id = 148, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MANDERA", StateId = 24, Title = "MANDERA EAST" },
                    new City() { Id = 149, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MANDERA", StateId = 24, Title = "LAFEY" },
                   

                    //MARSABIT Cities
                    new City() { Id = 150, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MARSABIT", StateId = 25, Title = "MOYALE" },
                    new City() { Id = 151, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MARSABIT", StateId = 25, Title = "NORTH HORR" },
                    new City() { Id = 152, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MARSABIT", StateId = 25, Title = "SAKU" },
                    new City() { Id = 153, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MARSABIT", StateId = 25, Title = "LAISAMIS" },
                   

                    //MERU Cities
                    new City() { Id = 154, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "IGEMBE SOUTH" },
                    new City() { Id = 155, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "IGEMBE CENTRAL" },
                    new City() { Id = 156, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "IGEMBE NORTH" },
                    new City() { Id = 157, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "TIGANIA WEST" },
                    new City() { Id = 158, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "TIGANIA EAST" },
                    new City() { Id = 159, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "NORTH IMENTI" },
                    new City() { Id = 160, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "BUURI" },
                    new City() { Id = 161, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "CENTRAL IMENTI" },
                    new City() { Id = 162, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MERU", StateId = 26, Title = "SOUTH IMENTI" },
                  
                    //MIGORI Cities
                    new City() { Id = 163, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "RONGO" },
                    new City() { Id = 164, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "AWENDO" },
                    new City() { Id = 165, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "SUNA EAST" },
                    new City() { Id = 166, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "SUNA WEST" },
                    new City() { Id = 167, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "URIRI" },
                    new City() { Id = 168, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "NYATIKE" },
                    new City() { Id = 169, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "KURIA WEST" },
                    new City() { Id = 170, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MIGORI", StateId = 27, Title = "KURIA EAST" },


                    //MOMBASA Cities
                    new City() { Id = 171, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MOMBASA", StateId = 28, Title = "CHANGAMWE" },
                    new City() { Id = 172, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MOMBASA", StateId = 28, Title = "JOMVU" },
                    new City() { Id = 173, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MOMBASA", StateId = 28, Title = "KISAUNI" },
                    new City() { Id = 174, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MOMBASA", StateId = 28, Title = "NYALI" },
                    new City() { Id = 175, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MOMBASA", StateId = 28, Title = "LIKONI" },
                    new City() { Id = 176, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MOMBASA", StateId = 28, Title = "MVITA" },


                    //MURANG'A Cities
                    new City() { Id = 177, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "KANGEMA" },
                    new City() { Id = 178, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "MATHIOYA" },
                    new City() { Id = 179, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "KIHARU" },
                    new City() { Id = 180, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "KIGUMO" },
                    new City() { Id = 181, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "MARAGWA" },
                    new City() { Id = 182, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "KANDARA" },
                    new City() { Id = 183, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "MURANG'A", StateId = 29, Title = "GATANGA" },


                    //NAIROBI Cities
                    new City() { Id = 184, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "WESTLANDS" },
                    new City() { Id = 185, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "DAGORETTI NORTH" },
                    new City() { Id = 186, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "DAGORETTI SOUTH" },
                    new City() { Id = 187, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "LANGATA" },
                    new City() { Id = 188, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "KIBRA" },
                    new City() { Id = 189, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "ROYSAMBU" },
                    new City() { Id = 190, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "KASARANI" },
                    new City() { Id = 191, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "RUARAKA" },
                    new City() { Id = 192, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "EMBAKASI SOUTH" },
                    new City() { Id = 193, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "EMBAKASI NORTH" },
                    new City() { Id = 194, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "EMBAKASI CENTRAL" },
                    new City() { Id = 195, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "EMBAKASI EAST" },
                    new City() { Id = 196, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "EMBAKASI WEST" },
                    new City() { Id = 197, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "MAKADARA" },
                    new City() { Id = 198, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "KAMUKUNJI" },
                    new City() { Id = 199, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "STAREHE" },
                    new City() { Id = 200, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAIROBI", StateId = 30, Title = "MATHARE" },

                    //NAKURU Cities
                    new City() { Id = 201, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "MOLO" },
                    new City() { Id = 202, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "NJORO" },
                    new City() { Id = 203, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "NAIVASHA" },
                    new City() { Id = 204, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "GILGIL" },
                    new City() { Id = 205, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "KURESOI SOUTH" },
                    new City() { Id = 206, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "KURESOI NORTH" },
                    new City() { Id = 207, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "SUBUKIA" },
                    new City() { Id = 208, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "RONGAI" },
                    new City() { Id = 209, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "BAHATI" },
                    new City() { Id = 210, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "NAKURU TOWN WEST" },
                    new City() { Id = 211, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false ,StateName = "NAKURU", StateId = 31, Title = "NAKURU TOWN EAST" },


                    //NANDI Cities
                    new City() { Id = 212, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NANDI", StateId = 32, Title = "TINDERET" },
                    new City() { Id = 213, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NANDI", StateId = 32, Title = "ALDAI" },
                    new City() { Id = 214, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NANDI", StateId = 32, Title = "NANDI HILLS" },
                    new City() { Id = 215, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NANDI", StateId = 32, Title = "CHESUMEI" },
                    new City() { Id = 216, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NANDI", StateId = 32, Title = "EMGWEN" },
                    new City() { Id = 217, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NANDI", StateId = 32, Title = "MOSOP" },


                    //NAROK Cities
                    new City() { Id = 218, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAROK", StateId = 33, Title = "KILGORIS" },
                    new City() { Id = 219, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAROK", StateId = 33, Title = "EMURUA DIKIRR" },
                    new City() { Id = 220, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAROK", StateId = 33, Title = "NAROK NORTH" },
                    new City() { Id = 221, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAROK", StateId = 33, Title = "NAROK EAST" },
                    new City() { Id = 222, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAROK", StateId = 33, Title = "NAROK SOUTH" },
                    new City() { Id = 223, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NAROK", StateId = 33, Title = "NAROK WEST" },

                    //NYAMIRA Cities
                    new City() { Id = 224, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYAMIRA", StateId = 34, Title = "KITUTU MASABA" },
                    new City() { Id = 225, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYAMIRA", StateId = 34, Title = "WEST MUGIRANGO" },
                    new City() { Id = 226, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYAMIRA", StateId = 34, Title = "NORTH MUGIRANGO" },
                    new City() { Id = 227, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYAMIRA", StateId = 34, Title = "BORABU" },

                    //NYANDARUA Cities
                    new City() { Id = 228, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYANDARUA", StateId = 35, Title = "KINANGOP" },
                    new City() { Id = 229, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYANDARUA", StateId = 35, Title = "KIPIPIRI" },
                    new City() { Id = 230, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYANDARUA", StateId = 35, Title = "OL KALOU" },
                    new City() { Id = 231, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYANDARUA", StateId = 35, Title = "OL JOROK" },
                    new City() { Id = 232, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYANDARUA", StateId = 35, Title = "NDARAGWA" },

                    //NYERI Cities
                    new City() { Id = 233, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYERI", StateId = 36, Title = "TETU" },
                    new City() { Id = 234, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYERI", StateId = 36, Title = "KIENI" },
                    new City() { Id = 235, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYERI", StateId = 36, Title = "MATHIRA" },
                    new City() { Id = 236, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYERI", StateId = 36, Title = "OTHAYA" },
                    new City() { Id = 237, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYERI", StateId = 36, Title = "MUKURWEINI" },
                    new City() { Id = 238, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "NYERI", StateId = 36, Title = "NYERI TOWN" },

                    //SAMBURU Cities
                    new City() { Id = 239, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SAMBURU", StateId = 37, Title = "SAMBURU WEST" },
                    new City() { Id = 240, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SAMBURU", StateId = 37, Title = "SAMBURU NORTH" },
                    new City() { Id = 241, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SAMBURU", StateId = 37, Title = "SAMBURU EAST" },

                    //SIAYA Cities
                    new City() { Id = 242, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SIAYA", StateId = 38, Title = "UGENYA" },
                    new City() { Id = 243, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SIAYA", StateId = 38, Title = "UGUNJA" },
                    new City() { Id = 244, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SIAYA", StateId = 38, Title = "ALEGO USONGA" },
                    new City() { Id = 245, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SIAYA", StateId = 38, Title = "GEM" },
                    new City() { Id = 246, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SIAYA", StateId = 38, Title = "BONDO" },
                    new City() { Id = 247, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "SIAYA", StateId = 38, Title = "RARIEDA" },

                    //TAITA TAVETA Cities
                    new City() { Id = 248, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TAITA TAVETA", StateId = 39, Title = "TAVETA" },
                    new City() { Id = 249, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TAITA TAVETA", StateId = 39, Title = "WUNDANYI" },
                    new City() { Id = 250, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TAITA TAVETA", StateId = 39, Title = "MWATATE" },
                    new City() { Id = 251, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TAITA TAVETA", StateId = 39, Title = "VOI" },

                    //TANA RIVER Cities
                    new City() { Id = 290, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TANA RIVER", StateId = 40, Title = "GARSEN" },
                    new City() { Id = 252, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TANA RIVER", StateId = 40, Title = "GALOLE" },
                    new City() { Id = 253, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TANA RIVER", StateId = 40, Title = "BURA" },

                    //THARAKA-NITHI Cities
                    new City() { Id = 254, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "THARAKA-NITHI", StateId = 41, Title = "MAARA" },
                    new City() { Id = 255, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "THARAKA-NITHI", StateId = 41, Title = "CHUKA/IGAMBANG'OMBE" },
                    new City() { Id = 256, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "THARAKA-NITHI", StateId = 41, Title = "THARAKA" },

                    //TRANS NZOIA Cities
                    new City() { Id = 257, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TRANS NZOIA", StateId = 42, Title = "KWANZA" },
                    new City() { Id = 258, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TRANS NZOIA", StateId = 42, Title = "ENDEBESS" },
                    new City() { Id = 259, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TRANS NZOIA", StateId = 42, Title = "SABOTI" },
                    new City() { Id = 260, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TRANS NZOIA", StateId = 42, Title = "KIMININI" },
                    new City() { Id = 261, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TRANS NZOIA", StateId = 42, Title = "CHERANGANY" },

                    //TURKANA Cities
                    new City() { Id = 262, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TURKANA", StateId = 43, Title = "TURKANA NORTH" },
                    new City() { Id = 263, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TURKANA", StateId = 43, Title = "TURKANA WEST" },
                    new City() { Id = 264, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TURKANA", StateId = 43, Title = "TURKANA CENTRAL" },
                    new City() { Id = 265, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TURKANA", StateId = 43, Title = "LOIMA" },
                    new City() { Id = 266, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TURKANA", StateId = 43, Title = "TURKANA SOUTH" },
                    new City() { Id = 267, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "TURKANA", StateId = 43, Title = "TURKANA EAST" },

                    //UASIN GISHU Cities
                    new City() { Id = 268, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "UASIN GISHU", StateId = 44, Title = "SOY" },
                    new City() { Id = 269, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "UASIN GISHU", StateId = 44, Title = "TURBO" },
                    new City() { Id = 270, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "UASIN GISHU", StateId = 44, Title = "MOIBEN" },
                    new City() { Id = 271, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "UASIN GISHU", StateId = 44, Title = "AINABKOI" },
                    new City() { Id = 272, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "UASIN GISHU", StateId = 44, Title = "KAPSERET" },
                    new City() { Id = 273, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "UASIN GISHU", StateId = 44, Title = "KESSES" },


                    //VIHIGA Cities
                    new City() { Id = 274, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "VIHIGA", StateId = 45, Title = "VIHIGA" },
                    new City() { Id = 275, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "VIHIGA", StateId = 45, Title = "SABATIA" },
                    new City() { Id = 276, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "VIHIGA", StateId = 45, Title = "HAMISI" },
                    new City() { Id = 277, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "VIHIGA", StateId = 45, Title = "LUANDA" },
                    new City() { Id = 278, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "VIHIGA", StateId = 45, Title = "EMUHAYA" },


                    //WAJIR Cities
                    new City() { Id = 279, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WAJIR", StateId = 46, Title = "WAJIR NORTH" },
                    new City() { Id = 280, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WAJIR", StateId = 46, Title = "WAJIR EAST" },
                    new City() { Id = 281, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WAJIR", StateId = 46, Title = "TARBAJ" },
                    new City() { Id = 282, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WAJIR", StateId = 46, Title = "WAJIR WEST" },
                    new City() { Id = 283, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WAJIR", StateId = 46, Title = "ELDAS" },
                    new City() { Id = 284, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WAJIR", StateId = 46, Title = "WAJIR SOUTH" },



                    //WEST POKOT Cities
                    new City() { Id = 285, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WEST POKOT", StateId = 46, Title = "KAPENGURIA" },
                    new City() { Id = 286, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WEST POKOT", StateId = 46, Title = "SIGOR" },
                    new City() { Id = 287, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WEST POKOT", StateId = 46, Title = "KACHELIBA" },
                    new City() { Id = 288, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WEST POKOT", StateId = 46, Title = "POKOT SOUTH" },
                    new City() { Id = 289, DeliveryId = 1, DeliveryPrice = -1, IsSetDeliveryPrice = false, StateName = "WEST POKOT", StateId = 46, Title = "MARAKWET WEST" }

                );


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.MyUser)
                .WithMany(a => a.MyAddresses);

            modelBuilder.Entity<BlogCategory>()
                .HasData(new BlogCategory() { BlogCategoryId = 1, BlogOrder = 0, Title = "Others" });

        }
       
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<SubProductVariation> SubProductVariations { get; set; }
        public DbSet<ProductTechnicalContent> ProductTechnicalContents { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<SpecialDiscount> SpecialDiscount { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTags> BlogTags { get; set; }
        public DbSet<BlogContent> BlogContents { get; set; }
        public DbSet<HomePage> HomePage { get; set; }
        public DbSet<HomeSliderImages> HomeSliderImages { get; set; }
        public DbSet<MobileHomeSliderImages> MobileHomeSliderImages { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<AboutUsContent> AboutUsContents { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReplyComment> ReplyComments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Shopping> Shoppings { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogReplyComment> BlogReplyComments { get; set; }
        public DbSet<ContactMessage> Messages { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ReplyTicket> ReplyTickets { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<SEOSettings> SEOSettings { get; set; }
        public DbSet<HomeBanner> HomeBanners { get; set; }
       public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogPageBanner> BlogPageBanners { get; set; }
        public DbSet<FavoritProduct> FavoritProduct { get; set; }

        public DbSet<RelatedCategory> RelatedCategories { get; set; }
    }
}
