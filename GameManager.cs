using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Oyun Kaynaklarý")]
    public int kilise = 50;
    public int halk = 50;
    public int ordu = 50;
    public int hazine = 50;

    [Header("UI Elementleri")]
    public TextMeshProUGUI kiliseText;
    public TextMeshProUGUI halkText;
    public TextMeshProUGUI orduText;
    public TextMeshProUGUI hazineText;
    public TextMeshProUGUI kartMetniText;
    public Button evetButton;
    public Button hayirButton;
    

    [Header("Kart Listesi")]
    public List<Kart> kartDestesi = new List<Kart>();

    private int mevcutKartIndex = 0;
    private bool oyunBitti = false;

    // ==========================
    // KALE ÝLERLEME (YENÝ)
    // ==========================
    [Header("Kale Ýlerleme")]
    [Tooltip("Bu sahnede kaç kart çevrilince haritaya dönüp bir sonraki kaleyi açacaðýz? Otomatik ayarlanýr.")]
    public int hedefKartSayisi = 30;

    [Tooltip("Þu ana kadar kaç kart çevrildi?")]
    public int cevrilenKartSayisi = 0;

    [Header("Sahne Ýsimleri")]
    public string mapSceneName = "MapScene"; // Harita sahnenin adý

    [Header("Kale Bilgisi (Otomatik)")]
    public string castleId;      // Kale1/Kale2/Kale3/Kale4
    public string nextCastleId;  // Sýradaki açýlacak kale

    [System.Serializable]
    public class Kart
    {
        public string soru;
        public string evetMetin;
        public string hayirMetin;
        public int kiliseEvet;
        public int halkEvet;
        public int orduEvet;
        public int hazineEvet;
        public int kiliseHayir;
        public int halkHayir;
        public int orduHayir;
        public int hazineHayir;
       
    }

    void Start()
    {
        // Haritadan hangi kale seçildi bilgisini al
        castleId = PlayerPrefs.GetString("SelectedCastle", "Kale1");

        // Seçilen kaleye göre hedef ve sonraki kale ayarla
        if (castleId == "Kale1")
        {
            hedefKartSayisi = 30;
            nextCastleId = "Kale2";
        }
        else if (castleId == "Kale2")
        {
            hedefKartSayisi = 50;
            nextCastleId = "Kale3";
        }
        else if (castleId == "Kale3")
        {
            hedefKartSayisi = 100;
            nextCastleId = "Kale4";
        }
        else // Kale4
        {
            hedefKartSayisi = 999999; // pratikte bitmesin
            nextCastleId = "";
        }

        // Sayaç sýfýrla
        cevrilenKartSayisi = 0;

        KartlariOlustur();
        ButonlariAyarla();
        KaynaklariGuncelle();
        KartGoster(0);
    }

    void KartlariOlustur()
    {
        // KART 1-10: HALK & SOSYAL KONULAR
        KartlariEkle(1, "Halk açlýk çekiyor. Erzak daðýtalým mý?", "Evet", "Hayýr", 0, 20, 0, -15, 0, -15, 10, 0);
        KartlariEkle(2, "Salgýn hastalýk var! Karantina uygulayalým mý?", "Evet", "Hayýr", -5, -10, 0, -10, -15, -25, -5, -15);
        KartlariEkle(3, "Halk festival düzenlemek istiyor. Destekleyelim mi?", "Evet", "Hayýr", 5, 25, 0, -15, -5, -15, 0, 10);
        KartlariEkle(4, "Köylüler vergi indirimi istiyor. Kabul edelim mi?", "Evet", "Hayýr", 0, 15, 0, -20, 0, -10, 0, 15);
        KartlariEkle(5, "Halk yeni bir kanun istiyor. Çýkaralým mý?", "Evet", "Hayýr", -5, 20, -5, -10, 10, -15, 10, 5);
        KartlariEkle(6, "Halk kralý görmek istiyor. Resepsiyon düzenleyelim mi?", "Evet", "Hayýr", 0, 20, -5, -15, 0, -10, 5, 10);
        KartlariEkle(7, "Yollar bozuk. Tamir için bütçe ayýralým mý?", "Evet", "Hayýr", 0, 15, 0, -20, 0, -10, 0, 10);
        KartlariEkle(8, "Halk eðitim istiyor. Okul açalým mý?", "Evet", "Hayýr", 10, 20, 0, -25, -10, -15, 0, 10);
        KartlariEkle(9, "Çiftçiler tohum yardýmý istiyor. Verelim mi?", "Evet", "Hayýr", 0, 20, 0, -15, 0, -15, 0, 10);
        KartlariEkle(10, "Halk temiz su istiyor. Çeþme yaptýralým mý?", "Evet", "Hayýr", 5, 25, 0, -20, -5, -20, 0, 15);

        // KART 11-20: KÝLÝSE & DÝNÝ KONULAR
        KartlariEkle(11, "Kilise yeni katedral istiyor. Ýzin verelim mi?", "Evet", "Hayýr", 20, 0, 0, -25, -15, 10, 0, 0);
        KartlariEkle(12, "Dini liderler daha fazla yetki istiyor. Verelim mi?", "Evet", "Hayýr", 25, -10, -10, 0, -20, 10, 10, 0);
        KartlariEkle(13, "Kilise bilim adamlarýný sansürlemek istiyor. Ýzin verelim mi?", "Evet", "Hayýr", 20, -15, 0, 0, -15, 15, 0, 0);
        KartlariEkle(14, "Rahipler vergi muafiyeti istiyor. Kabul edelim mi?", "Evet", "Hayýr", 15, -5, 0, -10, -10, 5, 0, 10);
        KartlariEkle(15, "Kilise festival düzenlemek istiyor. Destekleyelim mi?", "Evet", "Hayýr", 20, 10, 0, -15, -15, -5, 0, 10);
        KartlariEkle(16, "Dini kitaplarýn çoðaltýlmasý için fon ayýralým mý?", "Evet", "Hayýr", 15, 5, 0, -20, -10, -5, 0, 15);
        KartlariEkle(17, "Kilise yeni bir manastýr istiyor. Ýzin verelim mi?", "Evet", "Hayýr", 25, -5, 0, -30, -20, 10, 0, 15);
        KartlariEkle(18, "Dini liderler askeri operasyona karþý. Dinleyelim mi?", "Evet", "Hayýr", 20, 10, -15, 0, -15, -10, 20, 0);
        KartlariEkle(19, "Kilise fakirlere yardým istiyor. Destekleyelim mi?", "Evet", "Hayýr", 15, 15, 0, -20, -10, -10, 0, 15);
        KartlariEkle(20, "Dini bayram için hazine yardýmý isteniyor. Verelim mi?", "Evet", "Hayýr", 20, 10, 0, -25, -15, -5, 0, 20);

        // KART 21-35: ORDU & SAVUNMA
        KartlariEkle(21, "Ordu yeni silahlar istiyor. Bütçe ayýralým mý?", "Evet", "Hayýr", 0, -10, 25, -20, 0, 5, -10, 10);
        KartlariEkle(22, "Komþu krallýk savaþ tehdidi savuruyor. Saldýralým mý?", "Saldýr", "Barýþ", -10, -15, 30, -25, 10, 15, -10, 10);
        KartlariEkle(23, "Ordu sýnýr kaleleri inþa etmek istiyor. Ýzin verelim mi?", "Evet", "Hayýr", 0, -5, 20, -20, 0, 5, -10, 10);
        KartlariEkle(24, "Asker maaþlarýný artýrmak istiyor. Kabul edelim mi?", "Evet", "Hayýr", 0, -5, 25, -20, 0, 5, -15, 15);
        KartlariEkle(25, "Düþman esirleri serbest býrakalým mý?", "Evet", "Hayýr", 15, 10, -20, 0, -10, -5, 15, 0);
        KartlariEkle(26, "Ordu yeni zýrh teknolojisi istiyor. Araþtýralým mý?", "Evet", "Hayýr", 0, 0, 20, -25, 0, 0, -10, 20);
        KartlariEkle(27, "Sýnýr devriyelerini artýralým mý?", "Evet", "Hayýr", 0, -10, 15, -15, 0, 5, -5, 10);
        KartlariEkle(28, "Askeri akademi açalým mý?", "Evet", "Hayýr", 0, -5, 25, -30, 0, 5, -10, 25);
        KartlariEkle(29, "Ordu düþman topraklarýna keþif gönderilsin mi?", "Evet", "Hayýr", 0, 0, 15, -10, 0, 0, -5, 5);
        KartlariEkle(30, "Askeri ittifak teklifi var. Kabul edelim mi?", "Evet", "Hayýr", 0, 5, 20, -15, 0, -5, -10, 10);
        KartlariEkle(31, "Ordu tatbikat yapmak istiyor. Ýzin verelim mi?", "Evet", "Hayýr", 0, -5, 15, -10, 0, 5, -5, 5);
        KartlariEkle(32, "Savaþ ganimetlerini halka daðýtalým mý?", "Evet", "Hayýr", 0, 20, -10, 0, 0, -10, 15, 10);
        KartlariEkle(33, "Orduya atlý birlik ekleyelim mi?", "Evet", "Hayýr", 0, -10, 20, -25, 0, 5, -10, 20);
        KartlariEkle(34, "Askeri istihbarat için fon ayýralým mý?", "Evet", "Hayýr", 0, 0, 15, -15, 0, 0, -5, 10);
        KartlariEkle(35, "Ordu garnizonlarýný güçlendirelim mi?", "Evet", "Hayýr", 0, -5, 20, -20, 0, 5, -10, 15);

        // KART 36-50: HAZÝNE & EKONOMÝ
        KartlariEkle(36, "Tüccarlar vergi indirimi istiyor. Kabul edelim mi?", "Evet", "Hayýr", 0, 10, 0, -20, 0, -5, 0, 15);
        KartlariEkle(37, "Hazine yetkilileri yeni vergi sistemi öneriyor. Uygulayalým mý?", "Evet", "Hayýr", 0, -10, 0, 25, 0, 10, 0, -15);
        KartlariEkle(38, "Kaþifler yeni topraklar keþfetti. Koloni kuralým mý?", "Kur", "Reddet", 10, -10, 15, -30, 0, 5, -5, 10);
        KartlariEkle(39, "Ticaret yollarýný geniþletelim mi?", "Evet", "Hayýr", 0, 15, 0, -25, 0, -10, 0, 20);
        KartlariEkle(40, "Altýn madeni iþletmesi açalým mý?", "Evet", "Hayýr", 0, -15, 0, 30, 0, 10, 0, -25);
        KartlariEkle(41, "Tüccarlara ticaret gemisi filosu kuralým mý?", "Evet", "Hayýr", 0, 10, 5, -35, 0, -5, -5, 30);
        KartlariEkle(42, "Pazar yerlerini büyütelim mi?", "Evet", "Hayýr", 0, 20, 0, -20, 0, -15, 0, 15);
        KartlariEkle(43, "Dýþ ticaret anlaþmasý imzalayalým mý?", "Evet", "Hayýr", 0, 15, 0, 10, 0, -10, 0, -5);
        KartlariEkle(44, "Hazine rezervlerini artýralým mý?", "Evet", "Hayýr", 0, -10, 0, 20, 0, 5, 0, -15);
        KartlariEkle(45, "Tüccar loncasý kurulsun mu?", "Evet", "Hayýr", 0, 15, 0, -15, 0, -10, 0, 10);
        KartlariEkle(46, "Para birimimizi deðiþtirelim mi?", "Evet", "Hayýr", 0, -5, 0, 15, 0, 10, 0, -10);
        KartlariEkle(47, "Ticaret fuarý düzenleyelim mi?", "Evet", "Hayýr", 0, 20, 0, -25, 0, -15, 0, 20);
        KartlariEkle(48, "Hazine bonosu çýkaralým mý?", "Evet", "Hayýr", 0, -5, 0, 25, 0, 10, 0, -20);
        KartlariEkle(49, "Ticaret filosunu modernize edelim mi?", "Evet", "Hayýr", 0, 10, 5, -30, 0, -5, -5, 25);
        KartlariEkle(50, "Ekonomik danýþman atayalým mý?", "Evet", "Hayýr", 0, 5, 0, 15, 0, -5, 0, -10);

        // KART 51-65: DÝPLOMASÝ & ÝLÝÞKÝLER
        KartlariEkle(51, "Komþu krallýkla evlilik ittifaký teklifi var. Kabul edelim mi?", "Evet", "Hayýr", 10, 10, 15, 10, -10, -10, -15, -10);
        KartlariEkle(52, "Elçilik açalým mý?", "Evet", "Hayýr", 0, 5, 0, -15, 0, -5, 0, 10);
        KartlariEkle(53, "Uluslararasý zirveye katýlalým mý?", "Evet", "Hayýr", 0, 10, 0, -20, 0, -10, 0, 15);
        KartlariEkle(54, "Düþman krallýkla barýþ görüþmesi yapalým mý?", "Evet", "Hayýr", 10, 15, -10, 0, -10, -15, 15, 10);
        KartlariEkle(55, "Müttefiklerimize askeri yardým gönderelim mi?", "Evet", "Hayýr", 0, -5, -15, -20, 0, 5, 10, 15);
        KartlariEkle(56, "Ticaret elçisi atayalým mý?", "Evet", "Hayýr", 0, 15, 0, -10, 0, -10, 0, 8);
        KartlariEkle(57, "Komþu krallýða ekonomik yardým yapalým mý?", "Evet", "Hayýr", 5, 10, 0, -25, -5, -5, 0, 20);
        KartlariEkle(58, "Uluslararasý ticaret anlaþmasý imzalayalým mý?", "Evet", "Hayýr", 0, 20, 0, 15, 0, -15, 0, -10);
        KartlariEkle(59, "Düþmanla esir takasý yapalým mý?", "Evet", "Hayýr", 0, 10, 5, 0, 0, -5, -5, 0);
        KartlariEkle(60, "Komþu krallýða teknoloji transferi yapalým mý?", "Evet", "Hayýr", 0, 5, -10, 10, 0, -5, 15, -5);
        KartlariEkle(61, "Uluslararasý dil okulu açalým mý?", "Evet", "Hayýr", 0, 15, 0, -20, 0, -10, 0, 15);
        KartlariEkle(62, "Dýþiþleri bakaný atayalým mý?", "Evet", "Hayýr", 0, 5, 0, -15, 0, -5, 0, 10);
        KartlariEkle(63, "Komþu krallýða kültürel heyet gönderelim mi?", "Evet", "Hayýr", 5, 15, 0, -10, -5, -10, 0, 8);
        KartlariEkle(64, "Uluslararasý ticaret fuarýna katýlalým mý?", "Evet", "Hayýr", 0, 20, 0, -25, 0, -15, 0, 20);
        KartlariEkle(65, "Dost krallýða askeri eðitim verelim mi?", "Evet", "Hayýr", 0, 5, -10, -15, 0, -5, 15, 10);

        // KART 66-80: TEKNOLOJÝ & GELÝÞÝM
        KartlariEkle(66, "Bilim akademisi kuralým mý?", "Evet", "Hayýr", -10, 15, 0, -30, 15, -10, 0, 25);
        KartlariEkle(67, "Yeni tarým teknikleri araþtýralým mý?", "Evet", "Hayýr", 0, 25, 0, -20, 0, -20, 0, 15);
        KartlariEkle(68, "Týp araþtýrmalarý için fon ayýralým mý?", "Evet", "Hayýr", 0, 20, 0, -25, 0, -15, 0, 20);
        KartlariEkle(69, "Matbaa teknolojisini getirtelim mi?", "Evet", "Hayýr", -15, 25, 0, -35, 20, -20, 0, 30);
        KartlariEkle(70, "Mühendislik okulu açalým mý?", "Evet", "Hayýr", 0, 15, 10, -30, 0, -10, -5, 25);
        KartlariEkle(71, "Haritacýlýk çalýþmalarý yaptýralým mý?", "Evet", "Hayýr", 0, 10, 5, -20, 0, -5, -5, 15);
        KartlariEkle(72, "Astronomi gözlemevi kuralým mý?", "Evet", "Hayýr", -10, 15, 0, -25, 15, -10, 0, 20);
        KartlariEkle(73, "Su deðirmenleri inþa edelim mi?", "Evet", "Hayýr", 0, 20, 0, -30, 0, -15, 0, 25);
        KartlariEkle(74, "Metalurji araþtýrmalarý yaptýralým mý?", "Evet", "Hayýr", 0, 10, 15, -25, 0, -5, -10, 20);
        KartlariEkle(75, "Eðitim sistemi reformu yapalým mý?", "Evet", "Hayýr", -5, 25, 0, -35, 10, -20, 0, 30);
        KartlariEkle(76, "Teknoloji transferi için yabancý uzman getirtelim mi?", "Evet", "Hayýr", 0, 15, 5, -30, 0, -10, -5, 25);
        KartlariEkle(77, "Kütüphane inþa edelim mi?", "Evet", "Hayýr", -5, 20, 0, -25, 10, -15, 0, 20);
        KartlariEkle(78, "Teknoloji fuarý düzenleyelim mi?", "Evet", "Hayýr", 0, 15, 0, -20, 0, -10, 0, 15);
        KartlariEkle(79, "Bilimsel keþif gezisi düzenleyelim mi?", "Evet", "Hayýr", 0, 10, 0, -25, 0, -5, 0, 20);
        KartlariEkle(80, "Teknoloji danýþma kurulu oluþturalým mý?", "Evet", "Hayýr", 0, 10, 0, -15, 0, -5, 0, 10);

        // KART 81-100: ÖZEL OLAYLAR
        KartlariEkle(81, "Büyücü sarayda sihir yapmak istiyor. Ýzin verelim mi?", "Evet", "Hayýr", -20, 5, 10, 0, 15, -5, 0, 0);
        KartlariEkle(82, "Gizemli bir yabancý gelecek tahminleri sunuyor. Dinleyelim mi?", "Evet", "Hayýr", -10, 0, 0, -15, 15, 0, 0, 10);
        KartlariEkle(83, "Halk arasýnda doðaüstü olaylar rapor ediliyor. Araþtýralým mý?", "Evet", "Hayýr", -5, -5, 0, -10, 10, 10, 0, 5);
        KartlariEkle(84, "Gizemli bir harita bulundu. Keþif gönderelim mi?", "Evet", "Hayýr", 0, 0, 5, -20, 0, 0, -5, 15);
        KartlariEkle(85, "Mistik bir taþýn güçlü olduðu söyleniyor. Araþtýralým mý?", "Evet", "Hayýr", -15, 0, 0, -25, 20, 0, 0, 20);
        KartlariEkle(86, "Bir kahin kraliyetin geleceðini okumak istiyor. Ýzin verelim mi?", "Evet", "Hayýr", -10, 0, 0, -15, 15, 0, 0, 10);
        KartlariEkle(87, "Gizemli bir bitki þifalý olduðu iddia ediliyor. Araþtýralým mý?", "Evet", "Hayýr", 0, 10, 0, -10, 0, -5, 0, 5);
        KartlariEkle(88, "Doðaüstü güçlere sahip olduðunu iddia eden biri var. Ýnceleyelim mi?", "Evet", "Hayýr", -10, -5, 0, -15, 15, 10, 0, 10);
        KartlariEkle(89, "Eski bir kehanet bulundu. Araþtýralým mý?", "Evet", "Hayýr", -5, 0, 0, -10, 10, 0, 0, 5);
        KartlariEkle(90, "Mistik bir ayin düzenlenmek isteniyor. Ýzin verelim mi?", "Evet", "Hayýr", -15, 5, 0, -20, 20, -5, 0, 15);
        KartlariEkle(91, "Gizemli bir yabancý altýn recipe sunuyor. Deneyelim mi?", "Evet", "Hayýr", 0, 0, 0, 25, 0, 0, 0, -20);
        KartlariEkle(92, "Doðaüstü yetenekleri olduðunu iddia eden bir þifacý var. Saraya alalým mý?", "Evet", "Hayýr", -10, 15, 0, -15, 15, -10, 0, 10);
        KartlariEkle(93, "Mistik bir kýlýç bulundu. Orduya verelim mi?", "Evet", "Hayýr", 0, 0, 20, 0, 0, 0, -15, 0);
        KartlariEkle(94, "Gizemli bir iksir yapýmý öðrenilmek isteniyor. Araþtýralým mý?", "Evet", "Hayýr", -10, 10, 0, -20, 15, -5, 0, 15);
        KartlariEkle(95, "Doðaüstü varlýklar görüldüðü iddia ediliyor. Araþtýrma ekibi gönderelim mi?", "Evet", "Hayýr", -5, -5, 0, -15, 10, 10, 0, 10);
        KartlariEkle(96, "Mistik bir mühür bulundu. Kullanýlsýn mý?", "Evet", "Hayýr", -10, 0, 0, 0, 15, 0, 0, 0);
        KartlariEkle(97, "Gizemli bir tapýnak keþfedildi. Kazý yapalým mý?", "Evet", "Hayýr", -5, 0, 0, -25, 10, 0, 0, 20);
        KartlariEkle(98, "Doðaüstü güçlerle iletiþim kurduðunu iddia eden biri var. Dinleyelim mi?", "Evet", "Hayýr", -15, 0, 0, -10, 20, 0, 0, 5);
        KartlariEkle(99, "Mistik bir kristal enerji yaydýðý söyleniyor. Ýnceleyelim mi?", "Evet", "Hayýr", -10, 0, 0, -15, 15, 0, 0, 10);
        KartlariEkle(100, "Gizemli bir kült lideri halký etkiliyor. Müdahale edelim mi?", "Evet", "Hayýr", 10, -10, 5, 0, -15, 15, -5, 0);
    }

    void KartlariEkle(int kartNo, string soru, string evetMetin, string hayirMetin,
                     int kiliseEvet, int halkEvet, int orduEvet, int hazineEvet,
                     int kiliseHayir, int halkHayir, int orduHayir, int hazineHayir)
    {
        Kart yeniKart = new Kart();
        yeniKart.soru = soru;
        yeniKart.evetMetin = evetMetin;
        yeniKart.hayirMetin = hayirMetin;
        yeniKart.kiliseEvet = kiliseEvet;
        yeniKart.halkEvet = halkEvet;
        yeniKart.orduEvet = orduEvet;
        yeniKart.hazineEvet = hazineEvet;
        yeniKart.kiliseHayir = kiliseHayir;
        yeniKart.halkHayir = halkHayir;
        yeniKart.orduHayir = orduHayir;
        yeniKart.hazineHayir = hazineHayir;

        kartDestesi.Add(yeniKart);
    }

    void ButonlariAyarla()
    {
        evetButton.onClick.RemoveAllListeners();
        hayirButton.onClick.RemoveAllListeners();
        evetButton.onClick.AddListener(EvetSecildi);
        hayirButton.onClick.AddListener(HayirSecildi);
    }

    void EvetSecildi()
    {
        if (oyunBitti) return;
        if (kartDestesi.Count == 0) return;

        Kart mevcutKart = kartDestesi[mevcutKartIndex];
        kilise += mevcutKart.kiliseEvet;
        halk += mevcutKart.halkEvet;
        ordu += mevcutKart.orduEvet;
        hazine += mevcutKart.hazineEvet;

        OyunKontrolEt();
        if (oyunBitti) return;

        mevcutKartIndex = (mevcutKartIndex + 1) % kartDestesi.Count;
        KaynaklariGuncelle();
        KartGoster(mevcutKartIndex);

        KartCevrildi(); // <<< YENÝ
    }

    void HayirSecildi()
    {
        if (oyunBitti) return;
        if (kartDestesi.Count == 0) return;

        Kart mevcutKart = kartDestesi[mevcutKartIndex];
        kilise += mevcutKart.kiliseHayir;
        halk += mevcutKart.halkHayir;
        ordu += mevcutKart.orduHayir;
        hazine += mevcutKart.hazineHayir;

        OyunKontrolEt();
        if (oyunBitti) return;

        mevcutKartIndex = (mevcutKartIndex + 1) % kartDestesi.Count;
        KaynaklariGuncelle();
        KartGoster(mevcutKartIndex);

        KartCevrildi(); // <<< YENÝ
    }

    // ==========================
    // HER KART SEÇÝMÝNDE SAY
    // ==========================
    void KartCevrildi()
    {
        cevrilenKartSayisi++;

        // Ýstersen debug:
        Debug.Log($"{castleId} => {cevrilenKartSayisi}/{hedefKartSayisi}");

        if (cevrilenKartSayisi >= hedefKartSayisi)
        {
            // Hedef tamam: bir sonraki kaleyi aç
            UnlockNextCastle();

            // Haritaya dön
            SceneManager.LoadScene(mapSceneName);
        }
    }

    void UnlockNextCastle()
    {
        if (!string.IsNullOrEmpty(nextCastleId))
        {
            PlayerPrefs.SetInt(nextCastleId, 1);
            PlayerPrefs.Save();
        }
    }

    void OyunKontrolEt()
    {
        if (kilise <= 0) OyunSonu("Kilise seni desteklemiyor! \nDini liderler tarafýndan tahttan indirildin!");
        else if (halk <= 0) OyunSonu("Halk seni sevmiyor! \nBir isyanla tahttan indirildin!");
        else if (ordu <= 0) OyunSonu("Ordu zayýf! \nDüþman krallýk ülkeyi iþgal etti!");
        else if (hazine <= 0) OyunSonu("Hazine boþ! \nEkonomik çöküþ yaþandý!");
        else if (kilise >= 100) OyunSonu("Kilise çok güçlü! \nDini liderler yönetimi ele geçirdi!");
        else if (halk >= 100) OyunSonu("Halk çok mutlu! \nDemokrasi ilan edildi, krallýk sona erdi!");
        else if (ordu >= 100) OyunSonu("Ordu çok güçlü! \nGeneraller askeri darbe yaptý!");
        else if (hazine >= 100) OyunSonu("Hazine aðzýna kadar dolu! \nEn zengin kral olarak tarihe geçtin!");
    }

    void OyunSonu(string mesaj)
    {
        oyunBitti = true;
        kartMetniText.text = mesaj + "\n\n(Oyun Bitti)";
        evetButton.interactable = false;
        hayirButton.interactable = false;
        KaynaklariGuncelle();

        // Ýstersen ölünce haritaya dönsün:
        // SceneManager.LoadScene(mapSceneName);
    }

    void KartGoster(int kartIndex)
    {
        if (kartIndex < kartDestesi.Count)
        {
            Kart kart = kartDestesi[kartIndex];
            kartMetniText.text = kart.soru;
            evetButton.GetComponentInChildren<TextMeshProUGUI>().text = kart.evetMetin;
            hayirButton.GetComponentInChildren<TextMeshProUGUI>().text = kart.hayirMetin;

          
        }
    }

    void KaynaklariGuncelle()
    {
        kiliseText.text = "Kilise: " + Mathf.Max(0, kilise);
        halkText.text = "Halk: " + Mathf.Max(0, halk);
        orduText.text = "Ordu: " + Mathf.Max(0, ordu);
        hazineText.text = "Hazine: " + Mathf.Max(0, hazine);
    }
}
