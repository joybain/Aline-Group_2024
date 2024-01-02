using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Student
/// </summary>
public class Student
{
    public string StudentId;
    public string FName;
    public string MName;
    public string LName;
    public string BirthDt;
    public string Sex;
    public Byte[] StdPhoto;
    public string PerLoc;
    public string PerDistCode;
    public string PerThanaCode;
    public string PerZipCode;
    public string MailLoc;
    public string MailDistCode;
    public string MailThanaCode;
    public string MailZipCode;
    public string Email;
    public string TelNo;
    public string MobileNo;
    public string Nationality;
    public string Passport;
    public string VisaType;
    public string VisaExpDate;
    public string ContPerson;
    public string ContRelate;
    public string ContPhone;
    public string ContMobile;
    public string ContAddress;
    public string FthName;
    public string FthEdu;
    public string FthOccup;
    public string FthOrg;
    public string FthDesig;
    public string FthTel;
    public string FthOthAct;
    public string MthName;
    public string MthEdu;
    public string MthOccup;
    public string MthOrg;
    public string MthDesig;
    public string MthTel;
    public string MthOthAct;
    public string PrevSch;
    public string PrevAdd;
    public string LastClass;
    public string ClassYear;
    public string ClassPos;
    public string ReasonLeave;
    public string PhysicProb;
    public string Allergic;
    public string ChildRcv1;
    public string RelRcv1;
    public string ChildRcv2;
    public string RelRcv2;
    public string ChildRcv3;
    public string RelRcv3;
    public string ChildRcv4;
    public string RelRcv4;
    public Byte[] FthPhoto;
    public Byte[] MthPhoto;
    public Byte[] GuardPhoto1;
    public Byte[] GuardPhoto2;
    public Byte[] GuardPhoto3;
    public Byte[] GuardPhoto4;
    public Byte[] StdCurPhoto;
    public string EntryUser;
    public string EntryDate;
    public string UpdateUser;
    public string UpdateDate;
    public string Religion;
    public string Status;
    public string InactiveDate;
    public string AdmissionDt;
    public string BloodGroup;
    public string StudentRealID;
    public Student()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Student(DataRow dr)
    {
        if (dr["blood_group"].ToString() != string.Empty) { this.BloodGroup = dr["blood_group"].ToString(); }
        if (dr["student_id"].ToString() != string.Empty) { this.StudentId = dr["student_id"].ToString(); }
        if (dr["f_name"].ToString() != string.Empty) { this.FName = dr["f_name"].ToString(); }
        if (dr["m_name"].ToString() != string.Empty) { this.MName = dr["m_name"].ToString(); }
        if (dr["l_name"].ToString() != string.Empty) { this.LName = dr["l_name"].ToString(); }
        if (dr["birth_dt"].ToString() != string.Empty) { this.BirthDt = dr["birth_dt"].ToString(); }
        if (dr["sex"].ToString() != string.Empty) { this.Sex = dr["sex"].ToString(); }
        if (dr["std_photo"].ToString() != string.Empty) { this.StdPhoto = (byte[])dr["std_photo"]; }
        if (dr["per_loc"].ToString() != string.Empty) { this.PerLoc = dr["per_loc"].ToString(); }
        if (dr["per_dist_code"].ToString() != string.Empty) { this.PerDistCode = dr["per_dist_code"].ToString(); }
        if (dr["per_thana_code"].ToString() != string.Empty) { this.PerThanaCode = dr["per_thana_code"].ToString(); }
        if (dr["per_zip_code"].ToString() != string.Empty) { this.PerZipCode = dr["per_zip_code"].ToString(); }
        if (dr["mail_loc"].ToString() != string.Empty) { this.MailLoc = dr["mail_loc"].ToString(); }
        if (dr["mail_dist_code"].ToString() != string.Empty) { this.MailDistCode = dr["mail_dist_code"].ToString(); }
        if (dr["mail_thana_code"].ToString() != string.Empty) { this.MailThanaCode = dr["mail_thana_code"].ToString(); }
        if (dr["mail_zip_code"].ToString() != string.Empty) { this.MailZipCode = dr["mail_zip_code"].ToString(); }
        if (dr["email"].ToString() != string.Empty) { this.Email = dr["email"].ToString(); }
        if (dr["tel_no"].ToString() != string.Empty) { this.TelNo = dr["tel_no"].ToString(); }
        if (dr["mobile_no"].ToString() != string.Empty) { this.MobileNo = dr["mobile_no"].ToString(); }
        if (dr["nationality"].ToString() != string.Empty) { this.Nationality = dr["nationality"].ToString(); }
        if (dr["passport"].ToString() != string.Empty) { this.Passport = dr["passport"].ToString(); }
        if (dr["visa_type"].ToString() != string.Empty) { this.VisaType = dr["visa_type"].ToString(); }
        if (dr["visa_exp_date"].ToString() != string.Empty) { this.VisaExpDate = dr["visa_exp_date"].ToString(); }
        if (dr["cont_person"].ToString() != string.Empty) { this.ContPerson = dr["cont_person"].ToString(); }
        if (dr["cont_relate"].ToString() != string.Empty) { this.ContRelate = dr["cont_relate"].ToString(); }
        if (dr["cont_phone"].ToString() != string.Empty) { this.ContPhone = dr["cont_phone"].ToString(); }
        if (dr["cont_mobile"].ToString() != string.Empty) { this.ContMobile = dr["cont_mobile"].ToString(); }
        if (dr["cont_address"].ToString() != string.Empty) { this.ContAddress = dr["cont_address"].ToString(); }
        if (dr["fth_name"].ToString() != string.Empty) { this.FthName = dr["fth_name"].ToString(); }
        if (dr["fth_edu"].ToString() != string.Empty) { this.FthEdu = dr["fth_edu"].ToString(); }
        if (dr["fth_occup"].ToString() != string.Empty) { this.FthOccup = dr["fth_occup"].ToString(); }
        if (dr["fth_org"].ToString() != string.Empty) { this.FthOrg = dr["fth_org"].ToString(); }
        if (dr["fth_tel"].ToString() != string.Empty) { this.FthTel = dr["fth_tel"].ToString(); }
        if (dr["fth_oth_act"].ToString() != string.Empty) { this.FthOthAct = dr["fth_oth_act"].ToString(); }
        if (dr["mth_name"].ToString() != string.Empty) { this.MthName = dr["mth_name"].ToString(); }
        if (dr["mth_edu"].ToString() != string.Empty) { this.MthEdu = dr["mth_edu"].ToString(); }
        if (dr["mth_occup"].ToString() != string.Empty) { this.MthOccup = dr["mth_occup"].ToString(); }
        if (dr["mth_org"].ToString() != string.Empty) { this.MthOrg = dr["mth_org"].ToString(); }
        if (dr["mth_tel"].ToString() != string.Empty) { this.MthTel = dr["mth_tel"].ToString(); }
        if (dr["mth_oth_act"].ToString() != string.Empty) { this.MthOthAct = dr["mth_oth_act"].ToString(); }
        if (dr["prev_sch"].ToString() != string.Empty) { this.PrevSch = dr["prev_sch"].ToString(); }
        if (dr["prev_add"].ToString() != string.Empty) { this.PrevAdd = dr["prev_add"].ToString(); }
        if (dr["last_class"].ToString() != string.Empty) { this.LastClass = dr["last_class"].ToString(); }
        if (dr["class_year"].ToString() != string.Empty) { this.ClassYear = dr["class_year"].ToString(); }
        if (dr["class_pos"].ToString() != string.Empty) { this.ClassPos = dr["class_pos"].ToString(); }
        if (dr["reason_leave"].ToString() != string.Empty) { this.ReasonLeave = dr["reason_leave"].ToString(); }
        if (dr["physic_prob"].ToString() != string.Empty) { this.PhysicProb = dr["physic_prob"].ToString(); }
        if (dr["allergic"].ToString() != string.Empty) { this.Allergic = dr["allergic"].ToString(); }
        if (dr["child_rcv1"].ToString() != string.Empty) { this.ChildRcv1 = dr["child_rcv1"].ToString(); }
        if (dr["rel_rcv1"].ToString() != string.Empty) { this.RelRcv1 = dr["rel_rcv1"].ToString(); }
        if (dr["child_rcv2"].ToString() != string.Empty) { this.ChildRcv2 = dr["child_rcv2"].ToString(); }
        if (dr["rel_rcv2"].ToString() != string.Empty) { this.RelRcv2 = dr["rel_rcv2"].ToString(); }
        if (dr["child_rcv3"].ToString() != string.Empty) { this.ChildRcv3 = dr["child_rcv3"].ToString(); }
        if (dr["rel_rcv3"].ToString() != string.Empty) { this.RelRcv3 = dr["rel_rcv3"].ToString(); }
        if (dr["child_rcv4"].ToString() != string.Empty) { this.ChildRcv4 = dr["child_rcv4"].ToString(); }
        if (dr["rel_rcv4"].ToString() != string.Empty) { this.RelRcv4 = dr["rel_rcv4"].ToString(); }
        if (dr["fth_photo"].ToString() != string.Empty) { this.FthPhoto = (byte[])dr["fth_photo"]; }
        if (dr["mth_photo"].ToString() != string.Empty) { this.MthPhoto = (byte[])dr["mth_photo"]; }
        if (dr["guard_photo1"].ToString() != string.Empty) { this.GuardPhoto1 = (byte[])dr["guard_photo1"]; }
        if (dr["guard_photo2"].ToString() != string.Empty) { this.GuardPhoto2 = (byte[])dr["guard_photo2"]; }
        if (dr["guard_photo3"].ToString() != string.Empty) { this.GuardPhoto3 = (byte[])dr["guard_photo3"]; }
        if (dr["guard_photo4"].ToString() != string.Empty) { this.GuardPhoto4 = (byte[])dr["guard_photo4"]; }
        if (dr["std_cur_photo"].ToString() != string.Empty) { this.StdCurPhoto = (byte[])dr["std_cur_photo"]; }
        if (dr["entry_user"].ToString() != string.Empty) { this.EntryUser = dr["entry_user"].ToString(); }
        if (dr["entry_date"].ToString() != string.Empty) { this.EntryDate = dr["entry_date"].ToString(); }
        if (dr["update_user"].ToString() != string.Empty) { this.UpdateUser = dr["update_user"].ToString(); }
        if (dr["update_date"].ToString() != string.Empty) { this.UpdateDate = dr["update_date"].ToString(); }
        if (dr["religion"].ToString() != string.Empty) { this.Religion = dr["religion"].ToString(); }
        if (dr["status"].ToString() != string.Empty) { this.Status = dr["status"].ToString(); }
        if (dr["inactive_date"].ToString() != string.Empty) { this.InactiveDate = dr["inactive_date"].ToString(); }
        if (dr["std_admission_date"].ToString() != string.Empty) { this.InactiveDate = dr["std_admission_date"].ToString(); }
        //if (dr["ID"].ToString() != string.Empty) { this.StudentRealID = dr["ID"].ToString(); }
    }
  
}
