﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using is_takip_proje.Entity;

namespace is_takip_proje.Formlar
{
    public partial class FrmPersoneller : Form
    {
        public FrmPersoneller()
        {
            InitializeComponent();
        }

        DbisTakipEntities db = new DbisTakipEntities();
        void Personeller()
        {
            var degerler = from x in db.TblPersonel
                           select new
                           {
                               x.ID,
                               x.Ad,
                               x.Soyad,
                               x.Mail,
                               x.Telefon,
                               x.Gorsel,
                               Departman = x.TblDepartmanlar.Ad,
                               x.Durum
                           };
            gridControl1.DataSource = degerler.Where(x => x.Durum == true).ToList();
        }

        private void FrmPersoneller_Load(object sender, EventArgs e)
        {
            Personeller();
            var departmanlar = (from x in db.TblDepartmanlar
                                select new {
                                    x.ID,
                                    x.Ad
                                            }).ToList();
            lookUpEdit1.Properties.ValueMember = "ID";
            lookUpEdit1.Properties.DisplayMember = "Ad";
            lookUpEdit1.Properties.DataSource = departmanlar;
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            Personeller();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            TblPersonel t = new TblPersonel();
            t.Ad = TxtAd.Text;
            t.Soyad = TxtSoyad.Text;
            t.Mail = TxtMail.Text;
            t.Telefon = TxtTelefon.Text;
            t.Gorsel = TxtGorsel.Text;
            t.Departman = int.Parse(lookUpEdit1.EditValue.ToString());
            db.TblPersonel.Add(t);
            db.SaveChanges();
            XtraMessageBox.Show("Yeni personel kaydı başarılı bir şekilde gerçekleşti", 
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Personeller();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            var x = int.Parse(TxtId.Text);
            var deger = db.TblPersonel.Find(x);
            deger.Durum = false;
            db.SaveChanges();
            XtraMessageBox.Show("Personel başarılı bir şekilde silindi, silinen personeller listesinden " +
                "tüm silinmiş personel bilgilerine ulaşabilirsiniz...", "Bilgi", MessageBoxButtons.OK, 
                MessageBoxIcon.Warning);
            Personeller();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            TxtId.Text = gridView1.GetFocusedRowCellValue("ID").ToString();
            TxtAd.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
            TxtSoyad.Text = gridView1.GetFocusedRowCellValue("Soyad").ToString();
            TxtMail.Text = gridView1.GetFocusedRowCellValue("Mail").ToString();
            TxtTelefon.Text = gridView1.GetFocusedRowCellValue("Telefon").ToString();
            TxtGorsel.Text = gridView1.GetFocusedRowCellValue("Gorsel").ToString();
            lookUpEdit1.Text = gridView1.GetFocusedRowCellValue("Departman").ToString();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int x = int.Parse(TxtId.Text);
            var deger = db.TblPersonel.Find(x);
            deger.Ad = TxtAd.Text;
            deger.Soyad = TxtSoyad.Text;
            deger.Mail = TxtMail.Text;
            deger.Telefon = TxtTelefon.Text;
            deger.Gorsel = TxtGorsel.Text;
            deger.Departman = int.Parse(lookUpEdit1.EditValue.ToString());
            db.SaveChanges();
            XtraMessageBox.Show("Personel başarılı bir şekilde güncellendi", 
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Question);
            Personeller();
        }
    }
}
