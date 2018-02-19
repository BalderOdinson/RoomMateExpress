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

            //var èrnomerec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Èrnomerec"
            //};

            //var èulinec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Èulinec"
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

            //var ferenšèica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Ferenšèica"
            //};

            //var folnegoviæevo = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Folnegoviæevo"
            //};

            //var gajnice = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Gajnice"
            //};

            //var graèani = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Graèani"
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

            //var jakuševec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Jakuševec"
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

            //var knežija = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Knežija"
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

            //var kustošija = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kustošija"
            //};

            //var kvatriæ = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Kvatriæ"
            //};

            //var lanište = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Lanište"
            //};

            //var luèko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Luèko"
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

            //var malešnica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Malešnica"
            //};

            //var markuševec = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Markuševec"
            //};

            //var medvešèak = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Medvešèak"
            //};

            //var mikuliæi = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Mikuliæi"
            //};

            //var mlinovi = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Mlinovi"
            //};

            //var pešèenica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Pešèenica"
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

            //var preèko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Preèko"
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

            //var rudeš = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Rudeš"
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

            //var šestine = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Šestine"
            //};

            //var sesvete = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sesvete"
            //};

            //var sigeèica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sigeèica"
            //};

            //var siget = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Siget"
            //};

            //var sloboština = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sloboština"
            //};

            //var sopot = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Sopot"
            //};

            //var špansko = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Špansko"
            //};

            //var središæe = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Središæe"
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

            //var trešnjevka = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trešnjevka"
            //};

            //var trnava = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trnava"
            //};

            //var trnovèica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Trnovèica"
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

            //var volovèica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Volovèica"
            //};

            //var voltino = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Voltino"
            //};

            //var vrapèe = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Vrapèe"
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

            //var zapruðe = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Zapruðe"
            //};

            //var zavrtnica = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Zavrtnica"
            //};

            //var žitnjak = new Neighborhood
            //{
            //    City = zagreb,
            //    Name = "Žitnjak"
            //};

            //zagreb.Neighborhoods = new List<Neighborhood> { blato, borongaj, borovje, botinec, brestje, brezovica, bukovac, buzin, centar, èrnomerec, èulinec, cvjetnoNaselje, dubec, dubrava, dugave, ferenšèica, folnegoviæevo, gajnice, graèani, ivanja, reka, jakuševec, jankomir, jarun, kajzerica, kanal, klara, knežija, kruge, ksaver, kustošija, kvatriæ, lanište, luèko, ljubljanica, maksimir, malešnica, markuševec, medvešèak, mikuliæi, mlinovi, pešèenica, podsused, poljanice, preèko, ravnice, remete, remetinec, retkovec, rudeš, savica, savskiGaj, šestine, sesvete, sigeèica, siget, sloboština, sopot, špansko, središæe, srednjaci, stenjevec, stupnik, sveta, nedelja, svetice, travno, trešnjevka, trnava, trnovèica, trnsko, trnje, trokut, utrina, veliko, polje, volovèica, voltino, vrapèe, vrbani, vrbik, vukomerec, zapruðe, zavrtnica, žitnjak };

            //context.Cities.AddOrUpdate(zagreb);

            //var split = new City
            //{
            //    Name = "Split"
            //};

            //var baèvice = new Neighborhood
            //{
            //    City = split,
            //    Name = "Baèvice"
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

            //var draèevac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Draèevac"
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

            //var dujmovaèa = new Neighborhood
            //{
            //    City = split,
            //    Name = "Dujmovaèa"
            //};

            //var firule = new Neighborhood
            //{
            //    City = split,
            //    Name = "Firule"
            //};

            //var glavièine = new Neighborhood
            //{
            //    City = split,
            //    Name = "Glavièine"
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

            //var križine = new Neighborhood
            //{
            //    City = split,
            //    Name = "Križine"
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

            //var luèac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Luèac"
            //};

            //var manuš = new Neighborhood
            //{
            //    City = split,
            //    Name = "Manuš"
            //};

            //var mejaši = new Neighborhood
            //{
            //    City = split,
            //    Name = "Mejaši"
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

            //var škrape = new Neighborhood
            //{
            //    City = split,
            //    Name = "Škrape"
            //};

            //var smokovik = new Neighborhood
            //{
            //    City = split,
            //    Name = "Smokovik"
            //};

            //var smrdeèac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Smrdeèac"
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

            //var suæidar = new Neighborhood
            //{
            //    City = split,
            //    Name = "Suæidar"
            //};

            //var sukojišan = new Neighborhood
            //{
            //    City = split,
            //    Name = "Sukojišan"
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

            //var varoš = new Neighborhood
            //{
            //    City = split,
            //    Name = "Varoš"
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

            //var žnjan = new Neighborhood
            //{
            //    City = split,
            //    Name = "Žnjan"
            //};

            //var zvonèac = new Neighborhood
            //{
            //    City = split,
            //    Name = "Zvonèac"
            //};

            //split.Neighborhoods = new List<Neighborhood> { baèvice, bilice, blatine, bol, brda, dobri, draèevac, dragovode, dujilovo, dujmovaèa, firule, glavièine, grad, gripe, kacunar, kila, kman, kopilica, križine, lokve, lora, lovret, lovrinac, luèac, manuš, mejaši, meje, mertojak, neslanovac, pazdigrad, plokite, poljud, pujanke, radunica, ravneNjive, sirobuja, skalice, škrape, smokovik, smrdeèac, spinut, stinice, suæidar, sukojišan, sustipan, table, trstenik, veli, varoš, visoka, vranjic, žnjan, zvonèac };

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
            //    Name = "Industrijska èetvrt"
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

            //var našièkoNaselje = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Našièko naselje"
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

            //var tvrða = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Tvrða"
            //};

            //var zapadnoPredgraðe = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Zapadno predgraðe"
            //};

            //var zelenoPolje = new Neighborhood
            //{
            //    City = osijek,
            //    Name = "Zeleno polje"
            //};

            //osijek.Neighborhoods = new List<Neighborhood> { borova, centarOsijek, dionica, donjiGrad, gornjiGrad, industrijskaCetvrt, jugI, jugII, našièkoNaselje, pampas, pustara, retfala, sjenjak, stadionskoNaselje, tvrða, zapadnoPredgraðe, zelenoPolje };

            //context.Cities.AddOrUpdate(osijek);
        }
    }
}
