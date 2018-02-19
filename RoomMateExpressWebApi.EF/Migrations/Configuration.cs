using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RoomMateExpressWebApi.EF.Models.RoomMateExpressDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(RoomMateExpressWebApi.EF.Models.RoomMateExpressDbContext context)
        {
            //var zagreb = new City
            //{
            //    Name = "Zagreb"
            //};

            //var blato = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Blato"
            //};

            //var borongaj = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Borongaj"
            //};

            //var borovje = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Borovje"
            //};

            //var botinec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Botinec"
            //};

            //var brestje = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Brestje"
            //};

            //var brezovica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Brezovica"
            //};

            //var bukovac = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Bukovac"
            //};

            //var buzin = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Buzin"
            //};

            //var centar = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Centar"
            //};

            //var �rnomerec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "�rnomerec"
            //};

            //var �ulinec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "�ulinec"
            //};

            //var cvjetnoNaselje = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Cvjetno naselje"
            //};

            //var dubec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Dubec"
            //};

            //var dubrava = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Dubrava"
            //};

            //var dugave = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Dugave"
            //};

            //var feren��ica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Feren��ica"
            //};

            //var folnegovi�evo = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Folnegovi�evo"
            //};

            //var gajnice = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Gajnice"
            //};

            //var gra�ani = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Gra�ani"
            //};

            //var ivanja = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Ivanja"
            //};

            //var reka = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Reka"
            //};

            //var jaku�evec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Jaku�evec"
            //};

            //var jankomir = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Jankomir"
            //};

            //var jarun = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Jarun"
            //};

            //var kajzerica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kajzerica"
            //};

            //var kanal = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kanal"
            //};

            //var klara = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Klara"
            //};

            //var kne�ija = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kne�ija"
            //};

            //var kruge = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kruge"
            //};

            //var ksaver = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Ksaver"
            //};

            //var kusto�ija = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kusto�ija"
            //};

            //var kvatri� = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kvatri�"
            //};

            //var lani�te = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Lani�te"
            //};

            //var lu�ko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Lu�ko"
            //};

            //var ljubljanica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Ljubljanica"
            //};

            //var maksimir = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Maksimir"
            //};

            //var male�nica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Male�nica"
            //};

            //var marku�evec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Marku�evec"
            //};

            //var medve��ak = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Medve��ak"
            //};

            //var mikuli�i = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Mikuli�i"
            //};

            //var mlinovi = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Mlinovi"
            //};

            //var pe��enica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Pe��enica"
            //};

            //var podsused = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Podsused"
            //};

            //var poljanice = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Poljanice"
            //};

            //var pre�ko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Pre�ko"
            //};

            //var ravnice = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Ravnice"
            //};

            //var remete = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Remete"
            //};

            //var remetinec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Remetinec"
            //};

            //var retkovec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Retkovec"
            //};

            //var rude� = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Rude�"
            //};

            //var savica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Savica"
            //};

            //var savskiGaj = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Savski gaj"
            //};

            //var �estine = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "�estine"
            //};

            //var sesvete = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sesvete"
            //};

            //var sige�ica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sige�ica"
            //};

            //var siget = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Siget"
            //};

            //var slobo�tina = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Slobo�tina"
            //};

            //var sopot = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sopot"
            //};

            //var �pansko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "�pansko"
            //};

            //var sredi��e = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sredi��e"
            //};

            //var srednjaci = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Srednjaci"
            //};

            //var stenjevec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Stenjevec"
            //};

            //var stupnik = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Stupnik"
            //};

            //var sveta = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sveta"
            //};

            //var nedelja = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Nedelja"
            //};

            //var svetice = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Svetice"
            //};

            //var travno = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Travno"
            //};

            //var tre�njevka = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Tre�njevka"
            //};

            //var trnava = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trnava"
            //};

            //var trnov�ica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trnov�ica"
            //};

            //var trnsko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trnsko"
            //};

            //var trnje = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trnje"
            //};

            //var trokut = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trokut"
            //};

            //var utrina = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Utrina"
            //};

            //var veliko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Veliko"
            //};

            //var polje = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Polje"
            //};

            //var volov�ica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Volov�ica"
            //};

            //var voltino = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Voltino"
            //};

            //var vrap�e = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Vrap�e"
            //};

            //var vrbani = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Vrbani"
            //};

            //var vrbik = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Vrbik"
            //};

            //var vukomerec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Vukomerec"
            //};

            //var zapru�e = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Zapru�e"
            //};

            //var zavrtnica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Zavrtnica"
            //};

            //var �itnjak = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "�itnjak"
            //};

            //zagreb.Neighborhoods = new List<Neighborhood> { blato, borongaj, borovje, botinec, brestje, brezovica, bukovac, buzin, centar, �rnomerec, �ulinec, cvjetnoNaselje, dubec, dubrava, dugave, feren��ica, folnegovi�evo, gajnice, gra�ani, ivanja, reka, jaku�evec, jankomir, jarun, kajzerica, kanal, klara, kne�ija, kruge, ksaver, kusto�ija, kvatri�, lani�te, lu�ko, ljubljanica, maksimir, male�nica, marku�evec, medve��ak, mikuli�i, mlinovi, pe��enica, podsused, poljanice, pre�ko, ravnice, remete, remetinec, retkovec, rude�, savica, savskiGaj, �estine, sesvete, sige�ica, siget, slobo�tina, sopot, �pansko, sredi��e, srednjaci, stenjevec, stupnik, sveta, nedelja, svetice, travno, tre�njevka, trnava, trnov�ica, trnsko, trnje, trokut, utrina, veliko, polje, volov�ica, voltino, vrap�e, vrbani, vrbik, vukomerec, zapru�e, zavrtnica, �itnjak };

            //context.Cities.AddOrUpdate(zagreb);

            //var split = new City
            //{
            //    Name = "Split"
            //};

            //var ba�vice = new Neighborhood
            //{
            //    City = split,
            //    Name = "Ba�vice"
            //};

            //var bilice = new Neighborhood
            //{
            //    City = split,
            //    Name = "Bilice"
            //};

            //var blatine = new Neighborhood
            //{
            //    City = split,
            //    Name = "Blatine"
            //};

            //var bol = new Neighborhood
            //{
            //    City = split,
            //    Name = "Bol"
            //};

            //var brda = new Neighborhood
            //{
            //    City = split,
            //    Name = "Brda"
            //};

            //var dobri = new Neighborhood
            //{
            //    City = split,
            //    Name = "Dobri"
            //};

            //var dra�evac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Dra�evac"
            //};

            //var dragovode = new Neighborhood
            //{
            //    City = split,
            //    Name = "Dragovode"
            //};

            //var dujilovo = new Neighborhood
            //{
            //    City = split,
            //    Name = "Dujilovo"
            //};

            //var dujmova�a = new Neighborhood
            //{
            //    City = split,
            //    Name = "Dujmova�a"
            //};

            //var firule = new Neighborhood
            //{
            //    City = split,
            //    Name = "Firule"
            //};

            //var glavi�ine = new Neighborhood
            //{
            //    City = split,
            //    Name = "Glavi�ine"
            //};

            //var grad = new Neighborhood
            //{
            //    City = split,
            //    Name = "Grad"
            //};

            //var gripe = new Neighborhood
            //{
            //    City = split,
            //    Name = "Gripe"
            //};

            //var kacunar = new Neighborhood
            //{
            //    City = split,
            //    Name = "Kacunar"
            //};

            //var kila = new Neighborhood
            //{
            //    City = split,
            //    Name = "Kila"
            //};

            //var kman = new Neighborhood
            //{
            //    City = split,
            //    Name = "Kman"
            //};

            //var kopilica = new Neighborhood
            //{
            //    City = split,
            //    Name = "Kopilica"
            //};

            //var kri�ine = new Neighborhood
            //{
            //    City = split,
            //    Name = "Kri�ine"
            //};

            //var lokve = new Neighborhood
            //{
            //    City = split,
            //    Name = "Lokve"
            //};

            //var lora = new Neighborhood
            //{
            //    City = split,
            //    Name = "Lora"
            //};

            //var lovret = new Neighborhood
            //{
            //    City = split,
            //    Name = "Lovret"
            //};

            //var lovrinac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Lovrinac"
            //};

            //var lu�ac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Lu�ac"
            //};

            //var manu� = new Neighborhood
            //{
            //    City = split,
            //    Name = "Manu�"
            //};

            //var meja�i = new Neighborhood
            //{
            //    City = split,
            //    Name = "Meja�i"
            //};

            //var meje = new Neighborhood
            //{
            //    City = split,
            //    Name = "Meje"
            //};

            //var mertojak = new Neighborhood
            //{
            //    City = split,
            //    Name = "Mertojak"
            //};

            //var neslanovac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Neslanovac"
            //};

            //var pazdigrad = new Neighborhood
            //{
            //    City = split,
            //    Name = "Pazdigrad"
            //};

            //var plokite = new Neighborhood
            //{
            //    City = split,
            //    Name = "Plokite"
            //};

            //var poljud = new Neighborhood
            //{
            //    City = split,
            //    Name = "Poljud"
            //};

            //var pujanke = new Neighborhood
            //{
            //    City = split,
            //    Name = "Pujanke"
            //};

            //var radunica = new Neighborhood
            //{
            //    City = split,
            //    Name = "Radunica"
            //};

            //var ravneNjive = new Neighborhood
            //{
            //    City = split,
            //    Name = "Ravne njive"
            //};

            //var sirobuja = new Neighborhood
            //{
            //    City = split,
            //    Name = "Sirobuja"
            //};

            //var skalice = new Neighborhood
            //{
            //    City = split,
            //    Name = "Skalice"
            //};

            //var �krape = new Neighborhood
            //{
            //    City = split,
            //    Name = "�krape"
            //};

            //var smokovik = new Neighborhood
            //{
            //    City = split,
            //    Name = "Smokovik"
            //};

            //var smrde�ac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Smrde�ac"
            //};

            //var spinut = new Neighborhood
            //{
            //    City = split,
            //    Name = "Spinut"
            //};

            //var stinice = new Neighborhood
            //{
            //    City = split,
            //    Name = "Stinice"
            //};

            //var su�idar = new Neighborhood
            //{
            //    City = split,
            //    Name = "Su�idar"
            //};

            //var sukoji�an = new Neighborhood
            //{
            //    City = split,
            //    Name = "Sukoji�an"
            //};

            //var sustipan = new Neighborhood
            //{
            //    City = split,
            //    Name = "Sustipan"
            //};

            //var table = new Neighborhood
            //{
            //    City = split,
            //    Name = "Table"
            //};

            //var trstenik = new Neighborhood
            //{
            //    City = split,
            //    Name = "Trstenik"
            //};

            //var veli = new Neighborhood
            //{
            //    City = split,
            //    Name = "Veli"
            //};

            //var varo� = new Neighborhood
            //{
            //    City = split,
            //    Name = "Varo�"
            //};

            //var visoka = new Neighborhood
            //{
            //    City = split,
            //    Name = "Visoka"
            //};

            //var vranjic = new Neighborhood
            //{
            //    City = split,
            //    Name = "Vranjic"
            //};

            //var �njan = new Neighborhood
            //{
            //    City = split,
            //    Name = "�njan"
            //};

            //var zvon�ac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Zvon�ac"
            //};

            //split.Neighborhoods = new List<Neighborhood> { ba�vice, bilice, blatine, bol, brda, dobri, dra�evac, dragovode, dujilovo, dujmova�a, firule, glavi�ine, grad, gripe, kacunar, kila, kman, kopilica, kri�ine, lokve, lora, lovret, lovrinac, lu�ac, manu�, meja�i, meje, mertojak, neslanovac, pazdigrad, plokite, poljud, pujanke, radunica, ravneNjive, sirobuja, skalice, �krape, smokovik, smrde�ac, spinut, stinice, su�idar, sukoji�an, sustipan, table, trstenik, veli, varo�, visoka, vranjic, �njan, zvon�ac };

            //context.Cities.AddOrUpdate(split);

            //var osijek = new City
            //{
            //    Name = "Osijek"
            //};

            //var borova = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Borova"
            //};

            //var centarOsijek = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Centar"
            //};

            //var dionica = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Dionica"
            //};

            //var donjiGrad = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Donji Grad"
            //};

            //var gornjiGrad = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Gornji Grad"
            //};

            //var industrijskaCetvrt = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Industrijska �etvrt"
            //};

            //var jugI = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Jug I"
            //};

            //var jugII = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Jug II"
            //};

            //var na�i�koNaselje = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Na�i�ko naselje"
            //};

            //var pampas = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Pampas"
            //};

            //var pustara = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Pustara"
            //};

            //var retfala = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Retfala"
            //};

            //var sjenjak = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Sjenjak"
            //};

            //var stadionskoNaselje = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Stadionsko naselje"
            //};

            //var tvr�a = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Tvr�a"
            //};

            //var zapadnoPredgra�e = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Zapadno predgra�e"
            //};

            //var zelenoPolje = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Zeleno polje"
            //};

            //osijek.Neighborhoods = new List<Neighborhood> { borova, centarOsijek, dionica, donjiGrad, gornjiGrad, industrijskaCetvrt, jugI, jugII, na�i�koNaselje, pampas, pustara, retfala, sjenjak, stadionskoNaselje, tvr�a, zapadnoPredgra�e, zelenoPolje };

            //context.Cities.AddOrUpdate(osijek);
        }
    }
}
