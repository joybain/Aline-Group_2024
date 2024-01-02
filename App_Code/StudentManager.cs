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
using System.Data.SqlClient;
using System.IO;
/// <summary>
/// Summary description for StudentManager
/// </summary>
/// 
using autouniv;

namespace KHSC
{
    public class StudentManager
    {
       
        public static string ConvertBytesToString(byte[] bytes)
        {
            string output = String.Empty;
            MemoryStream stream = new MemoryStream(bytes);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                output = reader.ReadToEnd();
            }
            return output;
        }
        public static byte[] ConvertStringToBytes(string input)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(input);
                writer.Flush();
            }
            return stream.ToArray();
        }
        public static byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert,
                                       System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms;
            System.Drawing.Image returnImage;
            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/img/noimage.jpg"), FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] bt = br.ReadBytes((int)fs.Length);
            if (byteArrayIn.Length == 0)
            {
                byteArrayIn = bt;
            }
            ms = new MemoryStream(byteArrayIn);
            returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public enum ResizeOptions
        {
            // Use fixed width & height without keeping the proportions
            ExactWidthAndHeight,

            // Use maximum width (as defined) and keeping the proportions
            MaxWidth,

            // Use maximum height (as defined) and keeping the proportions
            MaxHeight,

            // Use maximum width or height (the biggest) and keeping the proportions
            MaxWidthAndHeight
        }
        public static System.Drawing.Bitmap DoResize(System.Drawing.Bitmap originalImg, int widthInPixels, int heightInPixels)
        {
            System.Drawing.Bitmap bitmap;
            try
            {
                bitmap = new System.Drawing.Bitmap(widthInPixels, heightInPixels);
                using (System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap))
                {
                    // Quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphic.DrawImage(originalImg, 0, 0, widthInPixels, heightInPixels);
                    return bitmap;
                }
            }
            finally
            {
                if (originalImg != null)
                {
                    originalImg.Dispose();
                }
            }
        }
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Bitmap image, int width, int height, ResizeOptions resizeOptions)
        {
            float f_width;
            float f_height;
            float dim;
            switch (resizeOptions)
            {
                case ResizeOptions.ExactWidthAndHeight:
                    return DoResize(image, width, height);

                case ResizeOptions.MaxHeight:
                    f_width = image.Width;
                    f_height = image.Height;

                    if (f_height <= height)
                        return DoResize(image, (int)f_width, (int)f_height);

                    dim = f_width / f_height;
                    width = (int)((float)(height) * dim);
                    return DoResize(image, width, height);

                case ResizeOptions.MaxWidth:
                    f_width = image.Width;
                    f_height = image.Height;

                    if (f_width <= width)
                        return DoResize(image, (int)f_width, (int)f_height);

                    dim = f_width / f_height;
                    height = (int)((float)(width) / dim);
                    return DoResize(image, width, height);

                case ResizeOptions.MaxWidthAndHeight:
                    int tmpHeight = height;
                    int tmpWidth = width;
                    f_width = image.Width;
                    f_height = image.Height;

                    if (f_width <= width && f_height <= height)
                        return DoResize(image, (int)f_width, (int)f_height);

                    dim = f_width / f_height;

                    // Check if the width is ok
                    if (f_width < width)
                        width = (int)f_width;
                    height = (int)((float)(width) / dim);
                    // The width is too width
                    if (height > tmpHeight)
                    {
                        if (f_height < tmpHeight)
                            height = (int)f_height;
                        else
                            height = tmpHeight;
                        width = (int)((float)(height) * dim);
                    }
                    return DoResize(image, width, height);
                default:
                    return image;
            }
        }
        public static DataTable getStdPhoto(string std,string col)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select "+col+" from student_info where student_id= " + std + "";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");
            return dt;
        } 
        public static void CreateStd(Student std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string variables = "student_id, f_name, m_name, l_name, birth_dt, sex, per_loc, per_dist_code, per_thana_code, per_zip_code, mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, entry_user, entry_date, religion,status,inactive_date,std_admission_date,blood_group";
            string values = " '" + std.StudentId + "', '" + std.FName + "',  '" + std.MName + "',  '" + std.LName + "','" + std.BirthDt + "', " +
                " '" + std.Sex + "',  '" + std.PerLoc + "',  '" + std.PerDistCode + "',  '" + std.PerThanaCode + "',  '" + std.PerZipCode + "',  " +
                " '" + std.MailLoc + "',  '" + std.MailDistCode + "',  '" + std.MailThanaCode + "',  '" + std.MailZipCode + "',  '" + std.Email + "',  '" + std.TelNo + "',  '" + std.MobileNo + "',  " +
                " '" + std.Nationality + "',  '" + std.Passport + "',  '" + std.VisaType + "','" + std.VisaExpDate + "', " +
                " '" + std.ContPerson + "',  '" + std.ContRelate + "',  '" + std.ContPhone + "',  '" + std.ContMobile + "',  '" + std.ContAddress + "',  '" + std.FthName + "',  " +
                " '" + std.FthEdu + "',  '" + std.FthOccup + "',  '" + std.FthOrg + "',  '" + std.FthTel + "',  '" + std.FthOthAct + "',  '" + std.MthName + "',  '" + std.MthEdu + "',  " +
                " '" + std.MthOccup + "',  '" + std.MthOrg + "',  '" + std.MthTel + "',  '" + std.MthOthAct + "',  '" + std.PrevSch + "',  '" + std.PrevAdd + "',  '" + std.LastClass + "',  " +
                "'" + std.ClassYear + "','" + std.ClassPos + "', " +
                " '" + std.ReasonLeave + "',  '" + std.PhysicProb + "',  '" + std.Allergic + "',  '" + std.ChildRcv1 + "',  '" + std.RelRcv1 + "',  '" + std.ChildRcv2 + "',  '" + std.RelRcv2 + "',  " +
                " '" + std.ChildRcv3 + "',  '" + std.RelRcv3 + "',  '" + std.ChildRcv4 + "',  '" + std.RelRcv4 + "','" + std.EntryUser + "','" + std.EntryDate + "','" + std.Religion + "','" + std.Status + "',convert(datetime,nullif('" + std.InactiveDate + "',''),103), convert(datetime,nullif('" + std.AdmissionDt + "',''),103),'"+std.BloodGroup+"'  ";
            string query = "";

            if (std.StdPhoto != null)
            {
                if (std.StdPhoto.Length > 0)
                {
                    variables = variables + ",std_photo";
                    values = values + ",@img";
                }
            }
            if (std.StdCurPhoto != null)
            {
                if (std.StdCurPhoto.Length > 0)
                {
                    variables = variables + ",std_cur_photo";
                    values = values + ",@stdcur";
                }
            }
            if (std.FthPhoto != null)
            {
                if (std.FthPhoto.Length > 0)
                {
                    variables = variables + ",fth_photo";
                    values = values + ",@fth";
                }
            }
            if (std.MthPhoto != null)
            {
                if (std.MthPhoto.Length > 0)
                {
                    variables = variables + ",mth_photo";
                    values = values + ",@mth";
                }
            }
            if (std.GuardPhoto1 != null)
            {
                if (std.GuardPhoto1.Length > 0)
                {
                    variables = variables + ",guard_photo1";
                    values = values + ",@guard1";
                }
            }
            if (std.GuardPhoto2 != null)
            {
                if (std.GuardPhoto2.Length > 0)
                {
                    variables = variables + ",guard_photo2";
                    values = values + ",@guard2";
                }
            }
            if (std.GuardPhoto3 != null)
            {
                if (std.GuardPhoto3.Length > 0)
                {
                    variables = variables + ",guard_photo3";
                    values = values + ",@guard3";
                }
            }
            if (std.GuardPhoto4 != null)
            {
                if (std.GuardPhoto4.Length > 0)
                {
                    variables = variables + ",guard_photo4";
                    values = values + ",@guard4";
                }
            }
            query = " insert into student_info (" + variables + ")  values ( " + values + " )";
            SqlCommand cmnd;
            cmnd = new SqlCommand(query, sqlCon);
            SqlParameter img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "img";
            img.Value = std.StdPhoto;            
            cmnd.Parameters.Add(img);
            if (std.StdPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.StdPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }                
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "stdcur";
            img.Value = std.StdCurPhoto;
            cmnd.Parameters.Add(img);
            if (std.StdCurPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.StdCurPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }
            
            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "fth";
            img.Value = std.FthPhoto;
            cmnd.Parameters.Add(img);
            if (std.FthPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.FthPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "mth";
            img.Value = std.MthPhoto;
            cmnd.Parameters.Add(img);
            if (std.MthPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.MthPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard1";
            img.Value = std.GuardPhoto1;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto1 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto1.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard2";
            img.Value = std.GuardPhoto2;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto2 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto2.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard3";
            img.Value = std.GuardPhoto3;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto3 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto3.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard4";
            img.Value = std.GuardPhoto4;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto4 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto4.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            sqlCon.Open();
            cmnd.ExecuteNonQuery();
            sqlCon.Close();
        }
        public static void UpdateStd(Student std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string variables = "f_name= '" + std.FName + "', m_name= '" + std.MName + "',  l_name= '" + std.LName + "',  birth_dt='" + std.BirthDt + "', " +
                            " sex= '" + std.Sex + "',  per_loc= '" + std.PerLoc + "',  per_dist_code= '" + std.PerDistCode + "',  per_thana_code= '" + std.PerThanaCode + "',  per_zip_code= '" + std.PerZipCode + "',  " +
                            " mail_loc= '" + std.MailLoc + "',  mail_dist_code= '" + std.MailDistCode + "',  mail_thana_code= '" + std.MailThanaCode + "',  " +
                            " mail_zip_code= '" + std.MailZipCode + "',  email= '" + std.Email + "',  tel_no= '" + std.TelNo + "',  mobile_no= '" + std.MobileNo + "',  nationality= '" + std.Nationality + "',  " +
                            " passport= '" + std.Passport + "',  visa_type= '" + std.VisaType + "',  visa_exp_date='" + std.VisaExpDate + "', " +
                            " cont_person= '" + std.ContPerson + "',  cont_relate= '" + std.ContRelate + "',  cont_phone= '" + std.ContPhone + "',  cont_mobile= '" + std.ContMobile + "',  " +
                            " cont_address= '" + std.ContAddress + "',  fth_name= '" + std.FthName + "',  fth_edu= '" + std.FthEdu + "',  fth_occup= '" + std.FthOccup + "',  " +
                            " fth_org= '" + std.FthOrg + "',  fth_tel= '" + std.FthTel + "',  fth_oth_act= '" + std.FthOthAct + "',  mth_name= '" + std.MthName + "',  mth_edu= '" + std.MthEdu + "',  " +
                            " mth_occup= '" + std.MthOccup + "',  mth_org= '" + std.MthOrg + "',  mth_tel= '" + std.MthTel + "',  mth_oth_act= '" + std.MthOthAct + "',  " +
                            " prev_sch= '" + std.PrevSch + "',  prev_add= '" + std.PrevAdd + "',  last_class= '" + std.LastClass + "',  class_pos= '" + std.ClassPos + "', " +
                            " reason_leave= '" + std.ReasonLeave + "',  physic_prob= '" + std.PhysicProb + "',  rel_rcv1= '" + std.RelRcv1 + "',  " +
                            " child_rcv2= '" + std.ChildRcv2 + "',  rel_rcv2= '" + std.RelRcv2 + "',  child_rcv3= '" + std.ChildRcv3 + "',  rel_rcv3= '" + std.RelRcv3 + "',  " +
                            " child_rcv4= '" + std.ChildRcv4 + "',  rel_rcv4= '" + std.RelRcv4 + "',  update_user= '" + std.UpdateUser + "',  " +
                            " update_date= '" + std.UpdateDate + "', religion='" + std.Religion + "',status='" + std.Status + "',inactive_date =convert(datetime,nullif('" + std.InactiveDate + "',''),103),[std_admission_date]=convert(datetime,nullif('" + std.AdmissionDt + "',''),103),blood_group='"+std.BloodGroup+"' ";
            string query = "";

            if (std.StdPhoto != null)
            {
                if (std.StdPhoto.Length > 0)
                {
                    variables = variables + ",std_photo=@img";
                }
            }

            if (std.StdCurPhoto != null)
            {
                if (std.StdCurPhoto.Length > 0)
                {
                    variables = variables + ",std_cur_photo=@stdcur";
                }
            }

            if (std.FthPhoto != null)
            {
                if (std.FthPhoto.Length > 0)
                {
                    variables = variables + ",fth_photo=@fth";
                }
            }
            if (std.MthPhoto != null)
            {
                if (std.MthPhoto.Length > 0)
                {
                    variables = variables + ",mth_photo=@mth";
                }
            }
            if (std.GuardPhoto1 != null)
            {
                if (std.GuardPhoto1.Length > 0)
                {
                    variables = variables + ",guard_photo1=@guard1";
                }
            }
            if (std.GuardPhoto2 != null)
            {
                if (std.GuardPhoto2.Length > 0)
                {
                    variables = variables + ",guard_photo2=@guard2";
                }
            }
            if (std.GuardPhoto3 != null)
            {
                if (std.GuardPhoto3.Length > 0)
                {
                    variables = variables + ",guard_photo3=@guard3";
                }
            }
            if (std.GuardPhoto4 != null)
            {
                if (std.GuardPhoto4.Length > 0)
                {
                    variables = variables + ",guard_photo4=@guard4";
                }
            }

            query = " update student_info set " + variables + " where student_id='" + std.StudentId + "'";

            SqlCommand cmnd;
            cmnd = new SqlCommand(query, sqlCon);
            SqlParameter img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "img";
            img.Value = std.StdPhoto;
            cmnd.Parameters.Add(img);
            if (std.StdPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.StdPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "stdcur";
            img.Value = std.StdCurPhoto;
            cmnd.Parameters.Add(img);
            if (std.StdCurPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.StdCurPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "fth";
            img.Value = std.FthPhoto;
            cmnd.Parameters.Add(img);
            if (std.FthPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.FthPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "mth";
            img.Value = std.MthPhoto;
            cmnd.Parameters.Add(img);
            if (std.MthPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.MthPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard1";
            img.Value = std.GuardPhoto1;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto1 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto1.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard2";
            img.Value = std.GuardPhoto2;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto2 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto2.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard3";
            img.Value = std.GuardPhoto3;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto3 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto3.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "guard4";
            img.Value = std.GuardPhoto4;
            cmnd.Parameters.Add(img);
            if (std.GuardPhoto4 == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (std.GuardPhoto4.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }
            sqlCon.Open();
            cmnd.ExecuteNonQuery();
            sqlCon.Close();
        }
        public static void DeleteStd(string std)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from student_info where student_id='" + std + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Student getStdRealID(string stdid)
        {
            String connectionString = DataManager.OraConnString();
            //string query = "select student_id, f_name, m_name, l_name, convert(varchar,birth_dt,103) birth_dt, sex, std_photo, per_loc, per_dist_code, per_thana_code, per_zip_code, " +
            //    " mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, convert(varchar,visa_exp_date,103) visa_exp_date, " +
            //    " cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, " +
            //    " mth_edu, mth_occup, mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, " +
            //    " child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, " +
            //    " guard_photo4, entry_user, convert(varchar,entry_date,103) entry_date, update_user, convert(varchar,update_date,103) update_date,status,convert(varchar,inactive_date,103) inactive_date,religion, std_cur_photo from student_info where student_id='" + stdid + "'";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");

            string query = @"select a.ID,a.student_id, f_name, m_name, l_name,convert(varchar,birth_dt,103) birth_dt, sex, std_photo, per_loc, per_dist_code,
            per_thana_code, per_zip_code, mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport,
visa_type, convert(varchar,visa_exp_date,103) visa_exp_date,  cont_person, cont_relate, cont_phone, cont_mobile,a.mobile_no,
cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name,  mth_edu, mth_occup, mth_org, mth_tel, 
mth_oth_act, prev_sch, prev_add, last_class, a.class_year, class_pos, reason_leave, physic_prob, allergic,  child_rcv1, rel_rcv1, 
child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3,  
guard_photo4, entry_user, convert(varchar,entry_date,103) entry_date, update_user, convert(varchar,update_date,103) update_date,
status,convert(varchar,inactive_date,103) inactive_date,religion, std_cur_photo,b.shift,b.[version],b.group_name,b.sect,a.blood_group,b.std_admission_date
from student_info a
inner join dbo.std_current_status b on a.ID=b.student_id where a.student_id='" + stdid + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Student(dt.Rows[0]);
        }
        public static Student getStds(int ID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select ID,student_id,f_name,m_name,l_name,fh_name,mh_name,per_loc,per_dist_code,per_thana_code,  mail_loc,mail_dist_code,mail_thana_code,mail_post_code,zip_area_code, dept_id, e_mail,mobile_no,res_ph_no,convert(nvarchar,birth_dt,103)birth_dt,place_of_birth,nationality, sex,blood_group,  religion, conct_person,national_id,sessions,study_year,batch_no,convert(nvarchar,doa,103)doa,campus,status,entry_user,convert(date,entry_date,103)entry_date,update_user,convert(date,update_date,103)update_date,  std_photo,sections,admis_type,mig_from,major,DeptTypeID,RefID,RefName,RefMobile,FinancialInsolvency,FreedomFighters from student_info where ID='" + ID + "' and DeleteBy IS NULL ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Student(dt.Rows[0]);
        }
        public static Student getStd(string stdid)
        {
            String connectionString = DataManager.OraConnString();
            //string query = "select student_id, f_name, m_name, l_name, convert(varchar,birth_dt,103) birth_dt, sex, std_photo, per_loc, per_dist_code, per_thana_code, per_zip_code, " +
            //    " mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, convert(varchar,visa_exp_date,103) visa_exp_date, " +
            //    " cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, " +
            //    " mth_edu, mth_occup, mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, " +
            //    " child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, " +
            //    " guard_photo4, entry_user, convert(varchar,entry_date,103) entry_date, update_user, convert(varchar,update_date,103) update_date,status,convert(varchar,inactive_date,103) inactive_date,religion, std_cur_photo from student_info where student_id='" + stdid + "'";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");

            string query = @"select a.student_id, f_name, m_name, l_name,convert(varchar,birth_dt,103) birth_dt, sex, std_photo, per_loc, per_dist_code,
            per_thana_code, per_zip_code, mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport,
visa_type, convert(varchar,visa_exp_date,103) visa_exp_date,  cont_person, cont_relate, cont_phone, cont_mobile, 
cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name,  mth_edu, mth_occup, mth_org, mth_tel, 
mth_oth_act, prev_sch, prev_add, last_class, a.class_year, class_pos, reason_leave, physic_prob, allergic,  child_rcv1, rel_rcv1, 
child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3,  
guard_photo4, entry_user, convert(varchar,entry_date,103) entry_date, update_user, convert(varchar,update_date,103) update_date,
status,convert(varchar,inactive_date,103) inactive_date,religion, std_cur_photo,b.shift,b.[version],b.group_name,b.sect,a.blood_group,b.std_admission_date
from student_info a
inner join dbo.std_current_status b on a.ID=b.student_id where a.student_id='" + stdid + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Student(dt.Rows[0]);
        }
        public static Student getStdPAy(string stdid)
        {
            String connectionString = DataManager.OraConnString();
            //string query = "select student_id, f_name, m_name, l_name, convert(varchar,birth_dt,103) birth_dt, sex, std_photo, per_loc, per_dist_code, per_thana_code, per_zip_code, " +
            //    " mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, convert(varchar,visa_exp_date,103) visa_exp_date, " +
            //    " cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, " +
            //    " mth_edu, mth_occup, mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, " +
            //    " child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, " +
            //    " guard_photo4, entry_user, convert(varchar,entry_date,103) entry_date, update_user, convert(varchar,update_date,103) update_date,status,convert(varchar,inactive_date,103) inactive_date,religion, std_cur_photo from student_info where student_id='" + stdid + "'";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");

            string query = @"select a.student_id, f_name, m_name, l_name,convert(varchar,birth_dt,103) birth_dt, sex, std_photo, per_loc, per_dist_code,
            per_thana_code, per_zip_code, mail_loc, mail_dist_code, mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport,
visa_type, convert(varchar,visa_exp_date,103) visa_exp_date,  cont_person, cont_relate, cont_phone, cont_mobile, 
cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name,  mth_edu, mth_occup, mth_org, mth_tel, 
mth_oth_act, prev_sch, prev_add, last_class, a.class_year, class_pos, reason_leave, physic_prob, allergic,  child_rcv1, rel_rcv1, 
child_rcv2, rel_rcv2, child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3,  
guard_photo4, entry_user, convert(varchar,entry_date,103) entry_date, update_user, convert(varchar,update_date,103) update_date,
status,convert(varchar,inactive_date,103) inactive_date,religion, std_cur_photo,b.shift,b.[version],b.group_name,b.sect,a.blood_group,b.std_admission_date
from student_info a
inner join dbo.std_current_status b on a.ID=b.student_id where a.student_id='" + stdid + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Student(dt.Rows[0]);
        }
        public static DataTable getStds(string stdid, string name, string dob)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select student_id,(rtrim(ltrim(rtrim(f_name))+' '+ ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) name, " +
                " convert(varchar,birth_dt,103) dob, fth_name,mth_name " +
                " from student_info a where student_id like '%" + stdid + "%' and lower(rtrim(ltrim(rtrim(f_name))+' '+ " +
                " ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) like '%" + name.ToLower() + "%' ";
            //and convert(varchar,birth_dt,103) like nullif('" + dob + "','%') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Students");
            return dt;
        }
        public static DataTable getStdListBatchSectionDept(string dept, string batch, string sec)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select student_id,(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,a.dept_id,b.dept_name from student_info a,dept_info b where a.dept_id=b.dept_id and " +
            " batch_no='" + batch + "' and nullif(sections,'A')='" + sec + "' and a.dept_id='" + dept + "' and isnull(status,'1')='1'";
            //and convert(varchar,birth_dt,103) like nullif('" + dob + "','%') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Students");
            return dt;
        }
        public static string getStudentName(string cc)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select (rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id='" + cc + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }
        public static string getStudentNamesssss(string cc)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select (rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where ID='" + cc + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }
        //******************************************************** Change Sub Query For (Student Name) *****************************//
        public static DataTable GetAllStudentInformation()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"select t1.student_id as [Id],t1.f_name as [Name],t1.fth_name as [Father],t1.mth_name as [Mother],cs.class_name AS [CLASS NAME],si.shift_name,sn.sec_name As [Section],vi.version_name AS [Version],t2.std_roll AS [STUDENT ROLL],t1.mobile_no  from student_info t1
inner join std_current_status t2 on t2.student_id=t1.ID
inner join class_info cs on cs.class_id=t2.class_id
inner join shift_info si on si.shift_id=t2.shift
inner join section_info sn on sn.sec_id=t2.sect
inner join version_info vi on vi.version_id=t2.[version] order by ID desc ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
            return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }           
           
        }

        public static Student GetAllStudentInformationForSpecificStudent(string studentId)
        {
             String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT 
       [f_name]
      ,[m_name]
      ,[l_name]
      ,[birth_dt]
      ,[sex]
      ,ISNULL([std_photo],0)
      ,[per_loc]
      ,[per_dist_code]
      ,[per_thana_code]
      ,[per_zip_code]
      ,[mail_loc]
      ,[mail_dist_code]
      ,[mail_thana_code]
      ,[mail_zip_code]
      ,[email]
      ,[tel_no]
      ,[mobile_no]
      ,[nationality]
      ,[passport]
      ,[visa_type]
      ,[visa_exp_date]
      ,[cont_person]
      ,[cont_relate]
      ,[cont_phone]
      ,[cont_mobile]
      ,[cont_address]
      ,[fth_name]
      ,[fth_edu]
      ,[fth_occup]
      ,[fth_org]
      ,[fth_tel]
      ,[fth_oth_act]
      ,[mth_name]
      ,[mth_edu]
      ,[mth_occup]
      ,[mth_org]
      ,[mth_tel]
      ,[mth_oth_act]
      ,[prev_sch]
      ,[prev_add]
      ,[last_class]
      ,[class_year]
      ,[class_pos]
      ,[reason_leave]
      ,[physic_prob]
      ,[allergic]
      ,[child_rcv1]
      ,[rel_rcv1]
      ,[child_rcv2]
      ,[rel_rcv2]
      ,[child_rcv3]
      ,[rel_rcv3]
      ,[child_rcv4]
      ,[rel_rcv4]
      ,ISNULL([fth_photo],0)
      ,ISNULL([mth_photo],0)
      ,ISNULL([guard_photo1],0)
      ,ISNULL([guard_photo2],0)
      ,ISNULL([guard_photo3],0)
      ,ISNULL([guard_photo4],0)       
      ,[religion],ISNULL([std_cur_photo],0) 
      ,[student_id]  
      ,status  
      ,convert(varchar,inactive_date,103) inactive_date
      ,convert(varchar,std_admission_date,103) std_admission_date,blood_group,ID  FROM [student_info] WHERE [student_id]='" + studentId + "'and status=1";
                SqlCommand cmd = new SqlCommand(selectQuery,myConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                Student aStudentObj = new Student();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        aStudentObj.FName = reader[0].ToString();
                        aStudentObj.MName = reader[1].ToString();
                        aStudentObj.LName = reader[2].ToString();
                        aStudentObj.BirthDt = reader[3].ToString();
                        aStudentObj.Sex = reader[4].ToString();
                        aStudentObj.StdPhoto = (byte[])reader[5];
                        aStudentObj.PerLoc = reader[6].ToString();
                        aStudentObj.PerDistCode = reader[7].ToString();
                        aStudentObj.PerThanaCode = reader[8].ToString();
                        aStudentObj.PerZipCode = reader[9].ToString();
                        aStudentObj.MailLoc = reader[10].ToString();
                        aStudentObj.MailDistCode = reader[11].ToString();
                        aStudentObj.MailThanaCode = reader[12].ToString();
                        aStudentObj.MailZipCode = reader[13].ToString();
                        aStudentObj.Email = reader[14].ToString();
                        aStudentObj.TelNo = reader[15].ToString();
                        aStudentObj.MobileNo = reader[16].ToString();
                        aStudentObj.Nationality = reader[17].ToString();
                        aStudentObj.Passport = reader[18].ToString();
                        aStudentObj.VisaType = reader[19].ToString();
                        aStudentObj.VisaExpDate = reader[20].ToString();
                        aStudentObj.ContPerson = reader[21].ToString();
                        aStudentObj.ContRelate = reader[22].ToString();
                        aStudentObj.ContPhone = reader[23].ToString();
                        aStudentObj.ContMobile = reader[24].ToString();
                        aStudentObj.ContAddress = reader[25].ToString();
                        aStudentObj.FthName = reader[26].ToString();
                        aStudentObj.FthEdu = reader[27].ToString();
                        aStudentObj.FthOccup = reader[28].ToString();
                        aStudentObj.FthOrg = reader[29].ToString();
                        aStudentObj.FthTel = reader[30].ToString(); 
                        aStudentObj.FthOthAct = reader[31].ToString();
                        aStudentObj.MthName = reader[32].ToString();
                        aStudentObj.MthEdu = reader[33].ToString();
                        aStudentObj.MthOccup = reader[34].ToString();
                        aStudentObj.MthOrg = reader[35].ToString();
                        aStudentObj.MthTel = reader[36].ToString();
                        aStudentObj.MthOthAct = reader[37].ToString();
                        aStudentObj.PrevSch = reader[38].ToString();
                        aStudentObj.PrevAdd = reader[39].ToString();
                        aStudentObj.LastClass = reader[40].ToString();
                        aStudentObj.ClassYear = reader[41].ToString();
                        aStudentObj.ClassPos = reader[42].ToString();
                        aStudentObj.ReasonLeave = reader[43].ToString();
                        aStudentObj.PhysicProb = reader[44].ToString();
                        aStudentObj.Allergic = reader[45].ToString();
                        aStudentObj.ChildRcv1 = reader[46].ToString();
                        aStudentObj.RelRcv1 = reader[47].ToString();
                        aStudentObj.ChildRcv2 = reader[48].ToString();
                        aStudentObj.RelRcv2 = reader[49].ToString();
                        aStudentObj.ChildRcv3 = reader[50].ToString();
                        aStudentObj.RelRcv3 = reader[51].ToString();
                        aStudentObj.ChildRcv4 = reader[52].ToString();
                        aStudentObj.RelRcv4 = reader[53].ToString();
                        aStudentObj.FthPhoto = (byte[]) reader[54];
                        aStudentObj.MthPhoto = (byte[]) reader[55];
                        aStudentObj.GuardPhoto1 = (byte[]) reader[56];
                        aStudentObj.GuardPhoto2 = (byte[]) reader[57];
                        aStudentObj.GuardPhoto3 = (byte[]) reader[58];
                        aStudentObj.GuardPhoto4 = (byte[]) reader[59];
                        aStudentObj.Religion = reader[60].ToString();
                        aStudentObj.StdCurPhoto = (byte[])reader[61];
                        aStudentObj.StudentId = reader["student_id"].ToString();
                        aStudentObj.Status = reader["status"].ToString();
                        aStudentObj.InactiveDate = reader["inactive_date"].ToString();
                        aStudentObj.AdmissionDt = reader["std_admission_date"].ToString();
                        aStudentObj.BloodGroup = reader["blood_group"].ToString();
                        aStudentObj.StudentRealID = reader["ID"].ToString();
                    }
                }
                return aStudentObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }

        }

        public static string GetDistrict(string district)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT  [DISTRICT_NAME]  FROM [DISTRICT_CODE] WHERE [DISTRICT_CODE]='" + district + "'";
                SqlCommand cmd = new SqlCommand(selectQuery, myConnection);
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static string GetThana(string thana)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT  [THANA_NAME] FROM [THANA_CODE] WHERE [THANA_CODE]='" + thana + "'";
                SqlCommand cmd = new SqlCommand(selectQuery, myConnection);
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetAllDistrictInformation()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT  [DISTRICT_CODE],[DISTRICT_NAME]  FROM [DISTRICT_CODE]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "DISTRICT_CODE");
                DataTable table = ds.Tables["DISTRICT_CODE"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetAllThanaForSpecificDistric(string districtCode)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT  [THANA_CODE]
      ,[THANA_NAME]       
  FROM [THANA_CODE] WHERE [DISTRICT_CODE]='" + districtCode + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "DISTRICT_CODE");
                DataTable table = ds.Tables["DISTRICT_CODE"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static object GetAllThanaThana()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT  [THANA_CODE]
      ,[THANA_NAME]       
  FROM [THANA_CODE]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "DISTRICT_CODE");
                DataTable table = ds.Tables["DISTRICT_CODE"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countIsStudentHaveAnAccountInformation(string studentId, string year, string classInfo)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
           try
           {
               myConnection.Open();
               string count = @"SELECT  COUNT(*) FROM [tbl_account_admission_fee] WHERE [acc_ad_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([acc_ad_class],' ','')))='" + classInfo + "' AND [acc_ad_year]='" + year + "'";
               SqlCommand cmd=new SqlCommand(count,myConnection);
               return Convert.ToInt32((cmd.ExecuteScalar()));
           }
        
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double studentAccountAdmissionInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }



        public static double StudentAccountTutionFeeInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' and accs_year ='" + year + "'";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountComputerFeeInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountGeneratorFeeInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountLibraryChargeInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountCulturalProgramInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountSportsPrizeInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountSchoolMagazineInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountcharityInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountAdmissionInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command=new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*)  FROM [tbl_account_admission_fee] WHERE [acc_ad_ledger_id]='" + aStudentPayment.HeadId + "' AND [acc_ad_student_id]='" + aStudentPayment.Id + "' AND [acc_ad_class]='" + aStudentPayment.Class + "' AND [acc_ad_year]='" + aStudentPayment.Year + "'";
                count=Convert.ToInt32(command.ExecuteScalar());
                if(count>0)
                {
                    command.CommandText = @"UPDATE [tbl_account_admission_fee] SET  [acc_ad_total_paid_amount] =[acc_ad_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[acc_ad_due_amount] ='" + aStudentPayment.DueAmount + "',[acc_ad_update_on] =GETDATE()  ,[acc_ad_update_by] ='Admin' WHERE [acc_ad_ledger_id]='" + aStudentPayment.HeadId + "' AND [acc_ad_student_id]='" + aStudentPayment.Id + "' AND [acc_ad_class]='" + aStudentPayment.Class + "' AND [acc_ad_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_account_admission_fee]
           ([acc_ad_ledger_id]
           ,[acc_ad_student_id]
           ,[acc_ad_class]
           ,[acc_ad_year]
           ,[acc_ad_total_amount]
           ,[acc_ad_total_paid_amount]
           ,[acc_ad_due_amount]
           ,[acc_ad_posted_on]
           ,[acc_ad_posted_by]
           ,[acc_ad_update_on]
           ,[acc_ad_update_by]
           ,[acc_ad_remarks]
           ,[acc_ad_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_account_admission_details]
           ([acc_ad_de_sl_no]
           ,[acc_ad_de_ledger_id]
           ,[acc_ad_de_student_id]
           ,[acc_ad_de_class]
           ,[acc_ad_de_year]
           ,[acc_ad_de_total_amount]
           ,[acc_ad_de_total_paid_amount]
           ,[acc_ad_de_due_amount]
           ,[acc_ad_de_posted_on]
           ,[acc_ad_de_posted_by]
           ,[acc_ad_de_update_on]
           ,[acc_ad_de_update_by]
           ,[acc_ad_de_remarks]
           ,[acc_ad_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([acc_ad_de_sl_no]),0) FROM [tbl_account_admission_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();
                
                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousAccountInformation(string headid,string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [acc_ad_total_amount]
      ,[acc_ad_total_paid_amount]
      ,[acc_ad_due_amount]      
  FROM [tbl_account_admission_fee] WHERE [acc_ad_ledger_id]='" + headid + "' AND [acc_ad_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([acc_ad_class],' ','')))='" + curClass + "' AND [acc_ad_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery,myConnection);
                DataSet ds=new DataSet();
                da.Fill(ds, "tbl_account_admission_fee");
                DataTable table = ds.Tables["tbl_account_admission_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountMonthlyTutionFeeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*)  FROM [tbl_acc_monthly_tution_fee] WHERE [mo_tu_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [mo_tu_fee_student_id]='" + aStudentPayment.Id + "' AND [mo_tu_fee_class]='" + aStudentPayment.Class + "' AND [mo_tu_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_monthly_tution_fee] SET  [mo_tu_fee_total_paid_amount] =[mo_tu_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[mo_tu_fee_due_amount] ='" + aStudentPayment.DueAmount + "',[mo_tu_fee_update_on] =GETDATE()  ,[mo_tu_fee_update_by] ='Admin' WHERE [mo_tu_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [mo_tu_fee_student_id]='" + aStudentPayment.Id + "' AND [mo_tu_fee_class]='" + aStudentPayment.Class + "' AND [mo_tu_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_monthly_tution_fee]
           ([mo_tu_fee_ledger_id]
           ,[mo_tu_fee_student_id]
           ,[mo_tu_fee_class]
           ,[mo_tu_fee_year]
           ,[mo_tu_fee_total_amount]
           ,[mo_tu_fee_total_paid_amount]
           ,[mo_tu_fee_due_amount]
           ,[mo_tu_fee_posted_on]
           ,[mo_tu_fee_posted_by]
           ,[mo_tu_fee_update_on]
           ,[mo_tu_fee_update_by]
           ,[mo_tu_fee_remarks]
           ,[mo_tu_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_monthly_tution_fee_details]
           ([mo_tu_fee_de_sl_no]
           ,[mo_tu_fee_de_ledger_id]
           ,[mo_tu_fee_de_student_id]
           ,[mo_tu_fee_de_class]
           ,[mo_tu_fee_de_year]
           ,[mo_tu_fee_de_total_amount]
           ,[mo_tu_fee_de_total_paid_amount]
           ,[mo_tu_fee_de_due_amount]
           ,[mo_tu_fee_de_posted_on]
           ,[mo_tu_fee_de_posted_by]
           ,[mo_tu_fee_de_update_on]
           ,[mo_tu_fee_de_update_by]
           ,[mo_tu_fee_de_remarks]
           ,[mo_tu_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([mo_tu_fee_de_sl_no]),0) FROM [tbl_acc_monthly_tution_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousMonthlyFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [mo_tu_fee_total_amount]
      ,[mo_tu_fee_total_paid_amount]
      ,[mo_tu_fee_due_amount]      
  FROM [tbl_acc_monthly_tution_fee] WHERE [mo_tu_fee_ledger_id]='" + headid + "' AND [mo_tu_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([mo_tu_fee_class],' ','')))='" + curClass + "' AND [mo_tu_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_monthly_tution_fee");
                DataTable table = ds.Tables["tbl_acc_monthly_tution_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousComputerFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [year_co_fee_total_amount]
      ,[year_co_fee_total_paid_amount]
      ,[year_co_fee_due_amount]      
  FROM [tbl_acc_yearly_computer_fee] WHERE [year_co_fee_ledger_id]='" + headid + "' AND [year_co_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([year_co_fee_class],' ','')))='" + curClass + "' AND [year_co_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_computer_fee");
                DataTable table = ds.Tables["tbl_acc_yearly_computer_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousGeneratorFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [year_gen_fee_total_amount]
      ,[year_gen_fee_total_paid_amount]
      ,[year_gen_fee_due_amount]      
  FROM [tbl_acc_yearly_generator_fee] WHERE [year_gen_fee_ledger_id]='" + headid + "' AND [year_gen_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([year_gen_fee_class],' ','')))='" + curClass + "' AND [year_gen_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_generator_fee");
                DataTable table = ds.Tables["tbl_acc_yearly_generator_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousCulturalFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [year_cul_pro_fee_total_amount]
      ,[year_cul_pro_fee_total_paid_amount]
      ,[year_cul_pro_fee_due_amount]      
  FROM [tbl_acc_yearly_cultural_program_fee] WHERE [year_cul_pro_fee_ledger_id]='" + headid + "' AND [year_cul_pro_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([year_cul_pro_fee_class],' ','')))='" + curClass + "' AND [year_cul_pro_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_cultural_program_fee");
                DataTable table = ds.Tables["tbl_acc_yearly_cultural_program_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousSportsFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [ysp_fee_total_amount]
      ,[ysp_fee_total_paid_amount]
      ,[ysp_fee_due_amount]      
  FROM [tbl_acc_yearly_sports_prize_fee] WHERE [ysp_fee_ledger_id]='" + headid + "' AND [ysp_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([ysp_fee_class],' ','')))='" + curClass + "' AND [ysp_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_sports_prize_fee");
                DataTable table = ds.Tables["tbl_acc_yearly_sports_prize_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousSchoolMagazineFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [ysm_fee_total_amount]
      ,[ysm_fee_total_paid_amount]
      ,[ysm_fee_due_amount]      
  FROM [tbl_acc_yearly_school_magazine_fee] WHERE [ysm_fee_ledger_id]='" + headid + "' AND [ysm_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([ysm_fee_class],' ','')))='" + curClass + "' AND [ysm_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_school_magazine_fee");
                DataTable table = ds.Tables["tbl_acc_yearly_school_magazine_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousCharityFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [charity_fee_total_amount]
      ,[charity_fee_total_paid_amount]
      ,[charity_fee_due_amount]      
  FROM [tbl_acc_charity_fee] WHERE [charity_fee_ledger_id]='" + headid + "' AND [charity_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([charity_fee_class],' ','')))='" + curClass + "' AND [charity_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_charity_fee");
                DataTable table = ds.Tables["tbl_acc_charity_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousLibraryFeeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [year_lib_cha_fee_total_amount]
      ,[year_lib_cha_fee_total_paid_amount]
      ,[year_lib_cha_fee_due_amount]      
  FROM [tbl_acc_yearly_library_charge] WHERE [year_lib_cha_fee_ledger_id]='" + headid + "' AND [year_lib_cha_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([year_lib_cha_fee_class],' ','')))='" + curClass + "' AND [year_lib_cha_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_library_charge");
                DataTable table = ds.Tables["tbl_acc_yearly_library_charge"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountComputerFeeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*)  FROM [tbl_acc_yearly_computer_fee] WHERE [year_co_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_co_fee_student_id]='" + aStudentPayment.Id + "' AND [year_co_fee_class]='" + aStudentPayment.Class + "' AND [year_co_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_yearly_computer_fee] SET  [year_co_fee_total_paid_amount] =[year_co_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[year_co_fee_due_amount] ='" + aStudentPayment.DueAmount + "',[year_co_fee_update_on] =GETDATE()  ,[year_co_fee_update_by] ='Admin' WHERE [year_co_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_co_fee_student_id]='" + aStudentPayment.Id + "' AND [year_co_fee_class]='" + aStudentPayment.Class + "' AND [year_co_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_computer_fee]
           ([year_co_fee_ledger_id]
           ,[year_co_fee_student_id]
           ,[year_co_fee_class]
           ,[year_co_fee_year]
           ,[year_co_fee_total_amount]
           ,[year_co_fee_total_paid_amount]
           ,[year_co_fee_due_amount]
           ,[year_co_fee_posted_on]
           ,[year_co_fee_posted_by]
           ,[year_co_fee_update_on]
           ,[year_co_fee_update_by]
           ,[year_co_fee_remarks]
           ,[year_co_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_computer_fee_details]
           ([year_co_fee_de_sl_no]
           ,[year_co_fee_de_ledger_id]
           ,[year_co_fee_de_student_id]
           ,[year_co_fee_de_class]
           ,[year_co_fee_de_year]
           ,[year_co_fee_de_total_amount]
           ,[year_co_fee_de_total_paid_amount]
           ,[year_co_fee_de_due_amount]
           ,[year_co_fee_de_posted_on]
           ,[year_co_fee_de_posted_by]
           ,[year_co_fee_de_update_on]
           ,[year_co_fee_de_update_by]
           ,[year_co_fee_de_remarks]
           ,[year_co_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([year_co_fee_de_sl_no]),0) FROM [tbl_acc_yearly_computer_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountGeneratorFeeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*)  FROM [tbl_acc_yearly_generator_fee] WHERE [year_gen_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_gen_fee_student_id]='" + aStudentPayment.Id + "' AND [year_gen_fee_class]='" + aStudentPayment.Class + "' AND [year_gen_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_yearly_generator_fee] SET  [year_gen_fee_total_paid_amount] =[year_gen_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[year_gen_fee_due_amount] ='" + aStudentPayment.DueAmount + "',[year_gen_fee_update_on]=GETDATE()  ,[year_gen_fee_update_by]='Admin' WHERE [year_gen_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_gen_fee_student_id]='" + aStudentPayment.Id + "' AND [year_gen_fee_class]='" + aStudentPayment.Class + "' AND [year_gen_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_generator_fee]
           ([year_gen_fee_ledger_id]
           ,[year_gen_fee_student_id]
           ,[year_gen_fee_class]
           ,[year_gen_fee_year]
           ,[year_gen_fee_total_amount]
           ,[year_gen_fee_total_paid_amount]
           ,[year_gen_fee_due_amount]
           ,[year_gen_fee_posted_on]
           ,[year_gen_fee_posted_by]
           ,[year_gen_fee_update_on]
           ,[year_gen_fee_update_by]
           ,[year_gen_fee_remarks]
           ,[year_gen_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_generator_fee_details]
           ([year_gen_fee_de_sl_no]
           ,[year_gen_fee_de_ledger_id]
           ,[year_gen_fee_de_student_id]
           ,[year_gen_fee_de_class]
           ,[year_gen_fee_de_year]
           ,[year_gen_fee_de_total_amount]
           ,[year_gen_fee_de_total_paid_amount]
           ,[year_gen_fee_de_due_amount]
           ,[year_gen_fee_de_posted_on]
           ,[year_gen_fee_de_posted_by]
           ,[year_gen_fee_de_update_on]
           ,[year_gen_fee_de_update_by]
           ,[year_gen_fee_de_remarks]
           ,[year_gen_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([year_gen_fee_de_sl_no]),0) FROM [tbl_acc_yearly_generator_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountLibraryFeeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*)  FROM [tbl_acc_yearly_library_charge] WHERE [year_lib_cha_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_lib_cha_fee_student_id]='" + aStudentPayment.Id + "' AND [year_lib_cha_fee_class]='" + aStudentPayment.Class + "' AND [year_lib_cha_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_yearly_library_charge] SET  [year_lib_cha_fee_total_paid_amount] =[year_lib_cha_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[year_lib_cha_fee_due_amount]='" + aStudentPayment.DueAmount + "',[year_lib_cha_fee_update_on]=GETDATE()  ,[year_lib_cha_fee_update_by]='Admin' WHERE [year_lib_cha_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_lib_cha_fee_student_id]='" + aStudentPayment.Id + "' AND [year_lib_cha_fee_class]='" + aStudentPayment.Class + "' AND [year_lib_cha_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_library_charge]
           ([year_lib_cha_fee_ledger_id]
           ,[year_lib_cha_fee_student_id]
           ,[year_lib_cha_fee_class]
           ,[year_lib_cha_fee_year]
           ,[year_lib_cha_fee_total_amount]
           ,[year_lib_cha_fee_total_paid_amount]
           ,[year_lib_cha_fee_due_amount]
           ,[year_lib_cha_fee_posted_on]
           ,[year_lib_cha_fee_posted_by]
           ,[year_lib_cha_fee_update_on]
           ,[year_lib_cha_fee_update_by]
           ,[year_lib_cha_fee_remarks]
           ,[year_lib_cha_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_library_charge_details]
           ([year_lib_cha_de_sl_no]
           ,[year_lib_cha_de_ledger_id]
           ,[year_lib_cha_de_student_id]
           ,[year_lib_cha_de_class]
           ,[year_lib_cha_de_year]
           ,[year_lib_cha_de_total_amount]
           ,[year_lib_cha_de_total_paid_amount]
           ,[year_lib_cha_de_due_amount]
           ,[year_lib_cha_de_posted_on]
           ,[year_lib_cha_de_posted_by]
           ,[year_lib_cha_de_update_on]
           ,[year_lib_cha_de_update_by]
           ,[year_lib_cha_de_remarks]
           ,[year_lib_cha_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([year_lib_cha_de_sl_no]),0) FROM [tbl_acc_yearly_library_charge_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountCulturalProgramInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_acc_yearly_cultural_program_fee] WHERE [year_cul_pro_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_cul_pro_fee_student_id]='" + aStudentPayment.Id + "' AND [year_cul_pro_fee_class]='" + aStudentPayment.Class + "' AND [year_cul_pro_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_yearly_cultural_program_fee] SET [year_cul_pro_fee_total_paid_amount] =[year_cul_pro_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[year_cul_pro_fee_due_amount]='" + aStudentPayment.DueAmount + "',[year_cul_pro_fee_update_on]=GETDATE()  ,[year_cul_pro_fee_update_by]='Admin' WHERE [year_cul_pro_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [year_cul_pro_fee_student_id]='" + aStudentPayment.Id + "' AND [year_cul_pro_fee_class]='" + aStudentPayment.Class + "' AND [year_cul_pro_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_cultural_program_fee]
           ([year_cul_pro_fee_ledger_id]
           ,[year_cul_pro_fee_student_id]
           ,[year_cul_pro_fee_class]
           ,[year_cul_pro_fee_year]
           ,[year_cul_pro_fee_total_amount]
           ,[year_cul_pro_fee_total_paid_amount]
           ,[year_cul_pro_fee_due_amount]
           ,[year_cul_pro_fee_posted_on]
           ,[year_cul_pro_fee_posted_by]
           ,[year_cul_pro_fee_update_on]
           ,[year_cul_pro_fee_update_by]
           ,[year_cul_pro_fee_remarks]
           ,[year_cul_pro_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_cultural_program_fee_details]
           ([ycp_fee_de_sl_no]
           ,[ycp_fee_de_ledger_id]
           ,[ycp_fee_de_student_id]
           ,[ycp_fee_de_class]
           ,[ycp_fee_de_year]
           ,[ycp_fee_de_total_amount]
           ,[ycp_fee_de_total_paid_amount]
           ,[ycp_fee_de_due_amount]
           ,[ycp_fee_de_posted_on]
           ,[ycp_fee_de_posted_by]
           ,[ycp_fee_de_update_on]
           ,[ycp_fee_de_update_by]
           ,[ycp_fee_de_remarks]
           ,[ycp_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([ycp_fee_de_sl_no]),0) FROM [tbl_acc_yearly_cultural_program_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountSportsFeeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_acc_yearly_sports_prize_fee] WHERE [ysp_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [ysp_fee_student_id]='" + aStudentPayment.Id + "' AND [ysp_fee_class]='" + aStudentPayment.Class + "' AND [ysp_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_yearly_sports_prize_fee] SET [ysp_fee_total_paid_amount] =[ysp_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[ysp_fee_due_amount]='" + aStudentPayment.DueAmount + "',[ysp_fee_update_on]=GETDATE()  ,[ysp_fee_update_by]='Admin' WHERE [ysp_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [ysp_fee_student_id]='" + aStudentPayment.Id + "' AND [ysp_fee_class]='" + aStudentPayment.Class + "' AND [ysp_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_sports_prize_fee]
           ([ysp_fee_ledger_id]
           ,[ysp_fee_student_id]
           ,[ysp_fee_class]
           ,[ysp_fee_year]
           ,[ysp_fee_total_amount]
           ,[ysp_fee_total_paid_amount]
           ,[ysp_fee_due_amount]
           ,[ysp_fee_posted_on]
           ,[ysp_fee_posted_by]
           ,[ysp_fee_update_on]
           ,[ysp_fee_update_by]
           ,[ysp_fee_remarks]
           ,[ysp_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_sports_prize_fee_details]
           ([ysp_fee_de_sl_no]
           ,[ysp_fee_de_ledger_id]
           ,[ysp_fee_de_student_id]
           ,[ysp_fee_de_class]
           ,[ysp_fee_de_year]
           ,[ysp_fee_de_total_amount]
           ,[ysp_fee_de_total_paid_amount]
           ,[ysp_fee_de_due_amount]
           ,[ysp_fee_de_posted_on]
           ,[ysp_fee_de_posted_by]
           ,[ysp_fee_de_update_on]
           ,[ysp_fee_de_update_by]
           ,[ysp_fee_de_remarks]
           ,[ysp_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([ysp_fee_de_sl_no]),0) FROM [tbl_acc_yearly_sports_prize_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountSchoolMagazineFeeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_acc_yearly_school_magazine_fee] WHERE [ysm_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [ysm_fee_student_id]='" + aStudentPayment.Id + "' AND [ysm_fee_class]='" + aStudentPayment.Class + "' AND [ysm_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_yearly_school_magazine_fee] SET [ysm_fee_total_paid_amount] =[ysm_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[ysm_fee_due_amount]='" + aStudentPayment.DueAmount + "',[ysm_fee_update_on]=GETDATE()  ,[ysm_fee_update_by]='Admin' WHERE [ysm_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [ysm_fee_student_id]='" + aStudentPayment.Id + "' AND [ysm_fee_class]='" + aStudentPayment.Class + "' AND [ysm_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_school_magazine_fee]
           ([ysm_fee_ledger_id]
           ,[ysm_fee_student_id]
           ,[ysm_fee_class]
           ,[ysm_fee_year]
           ,[ysm_fee_total_amount]
           ,[ysm_fee_total_paid_amount]
           ,[ysm_fee_due_amount]
           ,[ysm_fee_posted_on]
           ,[ysm_fee_posted_by]
           ,[ysm_fee_update_on]
           ,[ysm_fee_update_by]
           ,[ysm_fee_remarks]
           ,[ysm_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_school_magazine_fee_details]
           ([ysm_fee_de_sl_no]
           ,[ysm_fee_de_ledger_id]
           ,[ysm_fee_de_student_id]
           ,[ysm_fee_de_class]
           ,[ysm_fee_de_year]
           ,[ysm_fee_de_total_amount]
           ,[ysm_fee_de_total_paid_amount]
           ,[ysm_fee_de_due_amount]
           ,[ysm_fee_de_posted_on]
           ,[ysm_fee_de_posted_by]
           ,[ysm_fee_de_update_on]
           ,[ysm_fee_de_update_by]
           ,[ysm_fee_de_remarks]
           ,[ysm_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([ysm_fee_de_sl_no]),0) FROM [tbl_acc_yearly_school_magazine_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountCharityInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_acc_charity_fee] WHERE [charity_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [charity_fee_student_id]='" + aStudentPayment.Id + "' AND [charity_fee_class]='" + aStudentPayment.Class + "' AND [charity_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_acc_charity_fee] SET [charity_fee_total_paid_amount] =[charity_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[charity_fee_due_amount]='" + aStudentPayment.DueAmount + "',[charity_fee_update_on]=GETDATE()  ,[charity_fee_update_by]='Admin' WHERE [charity_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [charity_fee_student_id]='" + aStudentPayment.Id + "' AND [charity_fee_class]='" + aStudentPayment.Class + "' AND [charity_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_charity_fee]
           ([charity_fee_ledger_id]
           ,[charity_fee_student_id]
           ,[charity_fee_class]
           ,[charity_fee_year]
           ,[charity_fee_total_amount]
           ,[charity_fee_total_paid_amount]
           ,[charity_fee_due_amount]
           ,[charity_fee_posted_on]
           ,[charity_fee_posted_by]
           ,[charity_fee_update_on]
           ,[charity_fee_update_by]
           ,[charity_fee_remarks]
           ,[charity_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_charity_fee_details]
           ([cha_fee_de_sl_no]
           ,[cha_fee_de_ledger_id]
           ,[cha_fee_de_student_id]
           ,[cha_fee_de_class]
           ,[cha_fee_de_year]
           ,[cha_fee_de_total_amount]
           ,[cha_fee_de_total_paid_amount]
           ,[cha_fee_de_due_amount]
           ,[cha_fee_de_posted_on]
           ,[cha_fee_de_posted_by]
           ,[cha_fee_de_update_on]
           ,[cha_fee_de_update_by]
           ,[cha_fee_de_remarks]
           ,[cha_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([cha_fee_de_sl_no]),0) FROM [tbl_acc_charity_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountTransportFeeTwoInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_transport_two_km_fee] WHERE [mttkm_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [mttkm_fee_fee_student_id]='" + aStudentPayment.Id + "' AND [mttkm_fee_fee_class]='" + aStudentPayment.Class + "' AND [mttkm_fee_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_transport_two_km_fee] SET [mttkm_fee_fee_total_paid_amount] =[mttkm_fee_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[mttkm_fee_fee_due_amount]='" + aStudentPayment.DueAmount + "',[mttkm_fee_fee_update_on]=GETDATE()  ,[mttkm_fee_fee_update_by]='Admin' WHERE [mttkm_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [mttkm_fee_fee_student_id]='" + aStudentPayment.Id + "' AND [mttkm_fee_fee_class]='" + aStudentPayment.Class + "' AND [mttkm_fee_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_transport_two_km_fee]
           ([mttkm_fee_ledger_id]
           ,[mttkm_fee_fee_student_id]
           ,[mttkm_fee_fee_class]
           ,[mttkm_fee_fee_year]
           ,[mttkm_fee_fee_total_amount]
           ,[mttkm_fee_fee_total_paid_amount]
           ,[mttkm_fee_fee_due_amount]
           ,[mttkm_fee_fee_posted_on]
           ,[mttkm_fee_fee_posted_by]
           ,[mttkm_fee_fee_update_on]
           ,[mttkm_fee_fee_update_by]
           ,[mttkm_fee_fee_remarks]
           ,[mttkm_fee_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_transport_two_km_fee_details]
           ([mottkm_fee_de_sl_no]
           ,[mottkm_fee_de_ledger_id]
           ,[mottkm_fee_de_student_id]
           ,[mottkm_fee_de_class]
           ,[mottkm_fee_de_year]
           ,[mottkm_fee_de_total_amount]
           ,[mottkm_fee_de_total_paid_amount]
           ,[mottkm_fee_de_due_amount]
           ,[mottkm_fee_de_posted_on]
           ,[mottkm_fee_de_posted_by]
           ,[mottkm_fee_de_update_on]
           ,[mottkm_fee_de_update_by]
           ,[mottkm_fee_de_remarks]
           ,[mottkm_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([mottkm_fee_de_sl_no]),0) FROM [tbl_transport_two_km_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountTransportFeeOthersInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_transport_others_fee] WHERE [mttkmot_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [mttkmot_fee_fee_student_id]='" + aStudentPayment.Id + "' AND [mttkmot_fee_fee_class]='" + aStudentPayment.Class + "' AND [mttkmot_fee_fee_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE [tbl_transport_others_fee] SET [mttkmot_fee_fee_total_paid_amount] =[mttkmot_fee_fee_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[mttkmot_fee_fee_due_amount]='" + aStudentPayment.DueAmount + "',[mttkmot_fee_fee_update_on]=GETDATE()  ,[mttkmot_fee_fee_update_by]='Admin' WHERE [mttkmot_fee_ledger_id]='" + aStudentPayment.HeadId + "' AND [mttkmot_fee_fee_student_id]='" + aStudentPayment.Id + "' AND [mttkmot_fee_fee_class]='" + aStudentPayment.Class + "' AND [mttkmot_fee_fee_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_transport_others_fee]
           ([mttkmot_fee_ledger_id]
           ,[mttkmot_fee_fee_student_id]
           ,[mttkmot_fee_fee_class]
           ,[mttkmot_fee_fee_year]
           ,[mttkmot_fee_fee_total_amount]
           ,[mttkmot_fee_fee_total_paid_amount]
           ,[mttkmot_fee_fee_due_amount]
           ,[mttkmot_fee_fee_posted_on]
           ,[mttkmot_fee_fee_posted_by]
           ,[mttkmot_fee_fee_update_on]
           ,[mttkmot_fee_fee_update_by]
           ,[mttkmot_fee_fee_remarks]
           ,[mttkmot_fee_fee_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_transport_others_fee_details]
           ([motot_fee_de_sl_no]
           ,[motot_fee_de_ledger_id]
           ,[motot_fee_de_student_id]
           ,[motot_fee_de_class]
           ,[motot_fee_de_year]
           ,[motot_fee_de_total_amount]
           ,[motot_fee_de_total_paid_amount]
           ,[motot_fee_de_due_amount]
           ,[motot_fee_de_posted_on]
           ,[motot_fee_de_posted_by]
           ,[motot_fee_de_update_on]
           ,[motot_fee_de_update_by]
           ,[motot_fee_de_remarks]
           ,[motot_fee_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([motot_fee_de_sl_no]),0) FROM [tbl_transport_others_fee_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousTransportFeeTwoInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [mttkm_fee_fee_total_amount]
      ,[mttkm_fee_fee_total_paid_amount]
      ,[mttkm_fee_fee_due_amount]      
  FROM [tbl_transport_two_km_fee] WHERE [mttkm_fee_ledger_id]='" + headid + "' AND [mttkm_fee_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([mttkm_fee_fee_class],' ','')))='" + curClass + "' AND [mttkm_fee_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_transport_two_km_fee");
                DataTable table = ds.Tables["tbl_transport_two_km_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccounttransporttowInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' and accs_year ='" + year + "'";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousTransportFeeOthersInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [mttkmot_fee_fee_total_amount]
      ,[mttkmot_fee_fee_total_paid_amount]
      ,[mttkmot_fee_fee_due_amount]      
  FROM [tbl_transport_others_fee] WHERE [mttkmot_fee_ledger_id]='" + headid + "' AND [mttkmot_fee_fee_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([mttkmot_fee_fee_class],' ','')))='" + curClass + "' AND [mttkmot_fee_fee_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_transport_others_fee");
                DataTable table = ds.Tables["tbl_transport_others_fee"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccounttransportOthersInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' and accs_year ='" + year + "'";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetstudentPreviousYearlyChargeInformation(string headid, string stdId, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"SELECT [acc_year_ch_total_amount]
      ,[acc_year_ch_total_paid_amount]
      ,[acc_year_ch_due_amount]      
  FROM [tbl_acc_yearly_charge] WHERE [acc_year_ch_ledger_id]='" + headid + "' AND [acc_year_ch_student_id]='" + stdId + "' AND RTRIM(LTRIM(Replace([acc_year_ch_class],' ','')))='" + curClass + "' AND [acc_year_ch_year]='" + year + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_yearly_charge");
                DataTable table = ds.Tables["tbl_acc_yearly_charge"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static double StudentAccountYearlyChargeInformation(string head, string curClass, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT  [accs_amount] FROM [tbl_acc_setup] where accs_head_id ='" + head + "' and RTRIM(LTRIM(Replace(accs_class_id,' ',''))) ='" + curClass + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToDouble((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public void SaveTheAccountYearlyChargeInformation(StudentPayment aStudentPayment)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlTransaction trnsection = null;
            try
            {
                myConnection.Open();
                trnsection = myConnection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = myConnection;
                command.Transaction = trnsection;
                int count;
                command.CommandText = @"SELECT COUNT(*) FROM [tbl_acc_yearly_charge] WHERE [acc_year_ch_ledger_id]='" + aStudentPayment.HeadId + "' AND [acc_year_ch_student_id]='" + aStudentPayment.Id + "' AND [acc_year_ch_class]='" + aStudentPayment.Class + "' AND [acc_year_ch_year]='" + aStudentPayment.Year + "'";
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    command.CommandText = @"UPDATE FROM [tbl_acc_yearly_charge] SET [acc_year_ch_total_paid_amount] =[acc_year_ch_total_paid_amount]+'" + aStudentPayment.PaidAmount + "',[acc_year_ch_due_amount]='" + aStudentPayment.DueAmount + "',[acc_year_ch_update_on]=GETDATE()  ,[acc_year_ch_update_by]='Admin' WHERE [acc_year_ch_ledger_id]='" + aStudentPayment.HeadId + "' AND [acc_year_ch_student_id]='" + aStudentPayment.Id + "' AND [acc_year_ch_class]='" + aStudentPayment.Class + "' AND [acc_year_ch_year]='" + aStudentPayment.Year + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"INSERT INTO [tbl_acc_yearly_charge]
           ([acc_year_ch_ledger_id]
           ,[acc_year_ch_student_id]
           ,[acc_year_ch_class]
           ,[acc_year_ch_year]
           ,[acc_year_ch_total_amount]
           ,[acc_year_ch_total_paid_amount]
           ,[acc_year_ch_due_amount]
           ,[acc_year_ch_posted_on]
           ,[acc_year_ch_posted_by]
           ,[acc_year_ch_update_on]
           ,[acc_year_ch_update_by]
           ,[acc_year_ch_remarks]
           ,[acc_year_ch_late_fee])
     VALUES
           ('" + aStudentPayment.HeadId + "' ,'" + aStudentPayment.Id + "' ,'" + aStudentPayment.Class + "' ,'" + aStudentPayment.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                    command.ExecuteNonQuery();
                }

                command.CommandText = @"INSERT INTO [tbl_acc_yearly_charge_details]
           ([acc_year_ch_de_sl_no]
           ,[acc_year_ch_de_ledger_id]
           ,[acc_year_ch_de_student_id]
           ,[acc_year_ch_de_class]
           ,[acc_year_ch_de_year]
           ,[acc_year_ch_de_total_amount]
           ,[acc_year_ch_de_total_paid_amount]
           ,[acc_year_ch_de_due_amount]
           ,[acc_year_ch_de_posted_on]
           ,[acc_year_ch_de_posted_by]
           ,[acc_year_ch_de_update_on]
           ,[acc_year_ch_de_update_by]
           ,[acc_year_ch_de_remarks]
           ,[acc_year_ch_de_late_fee])
     VALUES
           ((SELECT ISNULL(MAX([acc_year_ch_de_sl_no]),0) FROM [tbl_acc_yearly_charge_details])+1,'" + aStudentPayment.HeadId + "','" + aStudentPayment.Id + "','" + aStudentPayment.Class + "','" + aStudentPayment.Year + "','" + aStudentPayment.TotalAmount + "','" + aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount + "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' )";
                command.ExecuteNonQuery();

                trnsection.Commit();
            }

            catch (Exception ex)
            {
                trnsection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countCharityAnAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [charity_fee_total_amount] FROM [tbl_acc_charity_fee] WHERE [charity_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([charity_fee_class],' ','')))='" + classInfo + "' AND [charity_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countSchoolMagazineAnAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [ysm_fee_total_amount] FROM [tbl_acc_yearly_school_magazine_fee] WHERE [ysm_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([ysm_fee_class],' ','')))='" + classInfo + "' AND [ysm_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countSportsPrizeAnAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [ysp_fee_total_amount] FROM [tbl_acc_yearly_sports_prize_fee] WHERE [ysp_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([ysp_fee_class],' ','')))='" + classInfo + "' AND [ysp_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countCulturalProgramAnAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [year_cul_pro_fee_total_amount] FROM [tbl_acc_yearly_cultural_program_fee] WHERE [year_cul_pro_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([year_cul_pro_fee_class],' ','')))='" + classInfo + "' AND [year_cul_pro_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countLibraryChargeAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [year_lib_cha_fee_total_amount] FROM [tbl_acc_yearly_library_charge] WHERE [year_lib_cha_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([year_lib_cha_fee_class],' ','')))='" + classInfo + "' AND [year_lib_cha_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countGeneratorFeeAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [year_gen_fee_total_amount] FROM [tbl_acc_yearly_generator_fee] WHERE [year_gen_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([year_gen_fee_class],' ','')))='" + classInfo + "' AND [year_gen_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countTransportForOthersAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [mttkmot_fee_fee_total_amount] FROM [tbl_transport_others_fee] WHERE [mttkmot_fee_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([mttkmot_fee_fee_class],' ','')))='" + classInfo + "' AND [mttkmot_fee_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countTransportForTwoAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [mttkm_fee_fee_total_amount] FROM [tbl_transport_two_km_fee] WHERE [mttkm_fee_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([mttkm_fee_fee_class],' ','')))='" + classInfo + "' AND [mttkm_fee_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countYearlyChargeAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [acc_year_ch_total_amount] FROM [tbl_acc_yearly_charge] WHERE [acc_year_ch_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([acc_year_ch_class],' ','')))='" + classInfo + "' AND [acc_year_ch_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static int countComputerFeeAccountInformation(string studentId, string classInfo, string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string count = @"SELECT [year_co_fee_total_amount] FROM [tbl_acc_yearly_computer_fee] WHERE [year_co_fee_student_id]='" + studentId + "' AND RTRIM(LTRIM(Replace([year_co_fee_class],' ','')))='" + classInfo + "' AND [year_co_fee_year]='" + year + "' ";
                SqlCommand cmd = new SqlCommand(count, myConnection);
                return Convert.ToInt32((cmd.ExecuteScalar()));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        //***************************** New Code Show All Head Specefy Class //
        public static DataTable GetAllHeadInformation(string Class, string stud)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string selectQuery = @"select accs_id, accs_head_id, case when accs_head_id='ACC-000003' then 
  (SELECT [acch_name] FROM tbl_acc_head WHERE [acch_id]=a.[accs_head_id]) + ' x ' + convert(varchar,mon) + ' month(s)'  else (SELECT [acch_name] FROM tbl_acc_head WHERE [acch_id]=a.[accs_head_id]) end acc_hade_name, accs_class_id, case when accs_head_id='ACC-000003' then convert(decimal(13,2),accs_amount * mon) else convert(decimal(13,2),accs_amount) end accs_amount
   from tbl_acc_setup a , (select datediff(month,class_start,GETDATE()) mon from std_current_status where student_id='" + stud+"') b where RTRIM(LTRIM(Replace([accs_class_id],' ','')))='" + Class + "' or [accs_class_id]='All Class'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, myConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_acc_setup");
                DataTable table = ds.Tables["tbl_acc_setup"];
                return table;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        
        public static DataTable GetAllStudentSearchByClass(string ClassID, string SectionID, string VersionID, string ShiftID,string GroupID)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                string Parmeter = "";
                if (!string.IsNullOrEmpty(ClassID) && string.IsNullOrEmpty(SectionID) && string.IsNullOrEmpty(ShiftID) &&
                    string.IsNullOrEmpty(GroupID))
                {
                    Parmeter = "  WHERE  t2.class_id='" + ClassID + "' order by convert(int,(t2.std_roll)) asc  ";
                }
                else if (!string.IsNullOrEmpty(ClassID) && !string.IsNullOrEmpty(SectionID) && string.IsNullOrEmpty(ShiftID) &&
                   string.IsNullOrEmpty(GroupID))
                {
                    Parmeter = "  WHERE  t2.class_id='" + ClassID + "' AND t2.sect='" + SectionID + "' order by convert(int,(t2.std_roll)) asc  ";
                }
                else if (!string.IsNullOrEmpty(ClassID) && !string.IsNullOrEmpty(SectionID) && !string.IsNullOrEmpty(ShiftID) &&
                   string.IsNullOrEmpty(GroupID))
                {
                    Parmeter = "  WHERE  t2.class_id='" + ClassID + "' AND t2.sect='" + SectionID + "' AND t2.shift='" + ShiftID + "' order by convert(int,(t2.std_roll)) asc  ";
                }
                else if (!string.IsNullOrEmpty(ClassID) && !string.IsNullOrEmpty(SectionID) && !string.IsNullOrEmpty(ShiftID) &&
                   !string.IsNullOrEmpty(GroupID))
                {
                    Parmeter = "  WHERE  t2.class_id='" + ClassID + "' AND t2.sect='" + SectionID + "' AND t2.shift='" + ShiftID + "' AND t2.group_name='" + GroupID + "' order by convert(int,(t2.std_roll)) asc  ";
                }
                else if (!string.IsNullOrEmpty(ClassID) && string.IsNullOrEmpty(SectionID) && string.IsNullOrEmpty(ShiftID) &&
                  string.IsNullOrEmpty(GroupID))
                {
                    Parmeter = "  WHERE  t2.class_id='" + ClassID + "' t2.group_name='" + GroupID + "' order by convert(int,(t2.std_roll)) asc  ";
                }
                myConnection.Open();
                string Query = @" SELECT t1.[student_id] as [Id]
      ,([f_name]+' '+[m_name]+' '+[l_name]  )as [Name]
      ,[fth_name] as [Father]
      ,[mth_name] as [Mother]
      ,t5.class_name AS [CLASS NAME]
      ,t3.sec_name As [Section]  
,t2.std_roll AS [STUDENT ROLL] 
,t6.version_name AS [version] 
,t1.mobile_no
  FROM [student_info] t1  INNER JOIN std_current_status t2 ON  t1.ID=t2.student_id   LEFT JOIN dbo.class_info t5 on t5.id=t2.class_id LEFT JOIN section_info t3 on t3.id=t2.sect LEFT JOIN  shift_info t4 on t4.shift_id=t2.shift 
  LEFT JOIN version_info t6 on t6.id=t2.[version] " + Parmeter;
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetAllStudentInformation(string ClassId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                if (ClassId == "")
                {
                    string Query = @"SELECT [student_id] as [Id]
      ,([f_name]+' '+[m_name]+' '+[l_name]  )as [Name]
      ,[fth_name] as [Father]
      ,[mth_name] as [Mother]
      ,(select class_name from class_info where class_id=(SELECT [class_id] FROM [std_current_status] WHERE [student_id]=b.[student_id]))AS [CLASS NAME]
      ,(select sec_name from section_info where id=(SELECT [sect]  FROM [std_current_status] WHERE [student_id]=b.[student_id]))As [Section]  
,(SELECT [std_roll]  FROM [std_current_status] WHERE [student_id]=b.[student_id])AS [STUDENT ROLL] 
  FROM [student_info] b where b.status=1";
                    DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                    return dt;
                }
                else
                {
                    string Query = @"SELECT [student_id] as [Id]
      ,([f_name]+' '+[m_name]+' '+[l_name]  )as [Name]
      ,[fth_name] as [Father]
      ,[mth_name] as [Mother]
      ,(select class_name from class_info where class_id=(SELECT [class_id] FROM [std_current_status] WHERE [student_id]=b.[student_id]))AS [CLASS NAME]
      ,(select sec_name from section_info where id=(SELECT [sect]  FROM [std_current_status] WHERE [student_id]=b.[student_id]))As [Section]  
,(SELECT [std_roll]  FROM [std_current_status] WHERE [student_id]=b.[student_id])AS [STUDENT ROLL] 
  FROM [student_info] b where (SELECT [class_id] FROM [std_current_status] WHERE [student_id]=b.[student_id])='" + ClassId + "' order by convert(int,(SELECT [std_roll]  FROM [std_current_status] WHERE [student_id]=b.[student_id])) asc ";
                    DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                    return dt;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        //************************* Show All Class **********************//
        public static DataTable GetAllStudentClassInformation()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT convert(int,class_id) as class_id ,[class_name] FROM  [class_info] order by convert(int,class_id) ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "class_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetAllEmployeeInformation()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"select em.emp_employee_id,em.emp_first_name from tbl_employee_information em order by em.emp_designation_id ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "class_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }


        public static DataTable GetClassWiseSubjectInformation(string ClassId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @" select ss.subject_id +'--'+ss.sub_name as sub_name,ss.subject_id  from subject_info ss where ss.offer_at='"+ClassId+"' order by ss.subject_id ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "class_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        //***************** Show Section On Class *******************//

        //public static object GetVersionOnClass(string classId)
        //{
        //    String ConnectionString = DataManager.OraConnString();
        //    SqlConnection myConnection = new SqlConnection(ConnectionString);
        //    try
        //    {
        //        myConnection.Open();
        //        string Query = @"SELECT * FROM version_info where class_id='" + classId + "'";
        //        DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "version_info");
        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (myConnection.State == ConnectionState.Open)
        //            myConnection.Close();
        //    }
        //}

        public static object GetVersionOnClass(string classId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM version_info where class_id='" + classId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "version_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static object GetShiftOnClass(string classId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM shift_info where class_id='" + classId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "shift_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        //*************************** version shift section show by class ****************************//
        public static object GetShowVersionOnClass(string classId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM version_info where class_id='" + classId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "version_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static object GetShowShiftOnClass(string classId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM shift_info where class_id='" + classId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "shift_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetShowSectionOnClass(string classId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM section_info where class_id='" + classId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "section_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }        


        public static object GetShowSection()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM section_info";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "section_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static object GetShiftInformation()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM shift_info";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "shift_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static object getVersionInfo()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT * FROM version_info";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "version_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }       

        //******************************************** Report Coding *********************************//
        public static DataTable getStudentIndividualInfo()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                          (SELECT DISTRICT_NAME FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) AS per_dist_code,(SELECT THANA_NAME FROM   THANA_CODE WHERE (THANA_CODE = a.per_thana_code)) AS per_thana_code, per_zip_code, mail_loc, (SELECT DISTRICT_NAME FROM DISTRICT_CODE AS DISTRICT_CODE_1 WHERE (DISTRICT_CODE = a.mail_dist_code)) AS mail_dist_code, (SELECT THANA_NAME FROM THANA_CODE AS THANA_CODE_1 WHERE (THANA_CODE = a.mail_thana_code)) AS mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, 
                      cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, 
                      mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, 
                      child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, guard_photo4, entry_user, entry_date, update_user, 
                      update_date, religion, std_cur_photo, (SELECT class_name
                            FROM class_info  WHERE (class_id = (SELECT class_id FROM std_current_status AS b WHERE (a.student_id = student_id)))) AS stcs_class, (SELECT class_year FROM std_current_status AS b WHERE (a.student_id = student_id)) AS stcs_year, (SELECT shift FROM std_current_status AS b WHERE(a.student_id = student_id)) AS stcs_shift, (SELECT sec_name FROM section_info  WHERE (sec_id = (SELECT sect FROM std_current_status AS b WHERE (a.student_id = student_id)))) AS stcs_section, (SELECT class_start FROM std_current_status AS b WHERE (a.student_id = student_id)) AS stcs_class_start, (SELECT std_roll FROM std_current_status AS b WHERE (a.student_id = student_id)) AS stcs_roll, (SELECT std_admission_date FROM  std_current_status AS b WHERE (a.student_id = student_id)) AS stcs_admission_date
FROM student_info AS a";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }


        public static DataTable getStudentInfoWithPicture(string classId,string sect,string shift)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT a.student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                           mail_loc,   mobile_no,  fth_name,   mth_name ,si.sec_name,ci.class_name,sc.std_roll                           
FROM student_info a 
inner join std_current_status sc on sc.student_id=a.ID inner join class_info ci on ci.class_id=sc.class_id and sc.class_id='" + classId + "' inner join section_info si on si.sec_id=sc.sect and sc.sect='" + sect + "' inner join shift_info sh on sh.shift_id=sc.shift and sc.shift='" + shift + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        //*********************** Std Picure Report **********************//
        public static byte[] StdImage(string Id)
        {
            byte[] img = null;
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = @"SELECT [std_photo] FROM  [student_info] Where student_id='" + Id + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue != System.DBNull.Value)
            {
                img = (byte[])maxValue;
            }
            return img;
        }


        //****** Search By Class ************//
        public static DataTable getStudentClassWiseReport(string Class)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                          (SELECT DISTRICT_NAME FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) AS per_dist_code,(SELECT     THANA_NAME FROM   THANA_CODE WHERE      (THANA_CODE = a.per_thana_code)) AS per_thana_code, per_zip_code, mail_loc, (SELECT     DISTRICT_NAME FROM DISTRICT_CODE AS DISTRICT_CODE_1 WHERE (DISTRICT_CODE = a.mail_dist_code)) AS mail_dist_code, (SELECT THANA_NAME FROM THANA_CODE AS THANA_CODE_1 WHERE      (THANA_CODE = a.mail_thana_code)) AS mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, 
                      cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, 
                      mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, 
                      child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, guard_photo4, entry_user, entry_date, update_user, 
                      update_date, religion, std_cur_photo, (SELECT     class_name
                            FROM class_info  WHERE (class_id = (SELECT class_id FROM std_current_status AS b  WHERE (a.ID = student_id)))) AS stcs_class, (SELECT class_year FROM std_current_status AS b WHERE (a.ID = student_id)) AS stcs_year, (SELECT shift FROM std_current_status AS b WHERE(a.ID = student_id)) AS stcs_shift, (SELECT sec_name FROM section_info  WHERE (sec_id = (SELECT     sect FROM          std_current_status AS b WHERE      (a.ID = student_id)))) AS stcs_section, (SELECT     class_start FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_class_start, (SELECT     std_roll FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_roll, (SELECT     std_admission_date FROM  std_current_status AS b WHERE (a.student_id = student_id)) AS stcs_admission_date
FROM student_info AS a WHERE (SELECT class_id FROM std_current_status AS b  WHERE  (a.ID = student_id)) LIKE'%" + Class + "%'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable getStudentClassAndSectionWiseReport(string Class, string Sec)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                          (SELECT DISTRICT_NAME FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) AS per_dist_code,(SELECT     THANA_NAME FROM   THANA_CODE WHERE      (THANA_CODE = a.per_thana_code)) AS per_thana_code, per_zip_code, mail_loc, (SELECT     DISTRICT_NAME FROM          DISTRICT_CODE AS DISTRICT_CODE_1 WHERE   (DISTRICT_CODE = a.mail_dist_code)) AS mail_dist_code, (SELECT     THANA_NAME FROM          THANA_CODE AS THANA_CODE_1 WHERE      (THANA_CODE = a.mail_thana_code)) AS mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, 
                      cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, 
                      mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, 
                      child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, guard_photo4, entry_user, entry_date, update_user, 
                      update_date, religion, std_cur_photo, (SELECT     class_name
                            FROM    class_info  WHERE      (class_id = (SELECT     class_id FROM          std_current_status AS b  WHERE      (a.student_id = student_id)))) AS stcs_class, (SELECT     class_year FROM   std_current_status AS b WHERE      (a.student_id = student_id)) AS stcs_year, (SELECT shift FROM          std_current_status AS b WHERE(a.student_id = student_id)) AS stcs_shift, (SELECT sec_name FROM          section_info  WHERE      (sec_id = (SELECT     sect FROM          std_current_status AS b WHERE      (a.student_id = student_id)))) AS stcs_section, (SELECT     class_start FROM          std_current_status AS b WHERE      (a.student_id = student_id)) AS stcs_class_start, (SELECT     std_roll FROM          std_current_status AS b WHERE      (a.student_id = student_id)) AS stcs_roll, (SELECT     std_admission_date FROM  std_current_status AS b WHERE (a.student_id = student_id)) AS stcs_admission_date
FROM student_info AS a WHERE (SELECT class_id FROM std_current_status AS b  WHERE  (a.student_id = student_id)) LIKE'%" + Class + "%' AND (SELECT  sect FROM          std_current_status AS b WHERE  (a.student_id = student_id)) LIKE'%"+Sec+"%'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable getStudentGenderWiseReport(string Gender)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                          (SELECT DISTRICT_NAME FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) AS per_dist_code,(SELECT     THANA_NAME FROM   THANA_CODE WHERE      (THANA_CODE = a.per_thana_code)) AS per_thana_code, per_zip_code, mail_loc, (SELECT     DISTRICT_NAME FROM          DISTRICT_CODE AS DISTRICT_CODE_1 WHERE   (DISTRICT_CODE = a.mail_dist_code)) AS mail_dist_code, (SELECT     THANA_NAME FROM          THANA_CODE AS THANA_CODE_1 WHERE      (THANA_CODE = a.mail_thana_code)) AS mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, 
                      cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, 
                      mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, 
                      child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, guard_photo4, entry_user, entry_date, update_user, 
                      update_date, religion, std_cur_photo, (SELECT     class_name
                            FROM    class_info  WHERE      (class_id = (SELECT     class_id FROM          std_current_status AS b  WHERE      (a.ID = student_id)))) AS stcs_class, (SELECT     class_year FROM   std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_year, (SELECT shift FROM          std_current_status AS b WHERE(a.ID = student_id)) AS stcs_shift, (SELECT sec_name FROM          section_info  WHERE      (sec_id = (SELECT     sect FROM          std_current_status AS b WHERE      (a.ID = student_id)))) AS stcs_section, (SELECT     class_start FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_class_start, (SELECT     std_roll FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_roll, (SELECT     std_admission_date FROM  std_current_status AS b WHERE (a.ID = student_id)) AS stcs_admission_date
FROM student_info AS a WHERE sex ='" + Gender + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable getStudentReligionWiseReport(string Religion)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                          (SELECT DISTRICT_NAME FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) AS per_dist_code,(SELECT     THANA_NAME FROM   THANA_CODE WHERE      (THANA_CODE = a.per_thana_code)) AS per_thana_code, per_zip_code, mail_loc, (SELECT     DISTRICT_NAME FROM          DISTRICT_CODE AS DISTRICT_CODE_1 WHERE   (DISTRICT_CODE = a.mail_dist_code)) AS mail_dist_code, (SELECT     THANA_NAME FROM          THANA_CODE AS THANA_CODE_1 WHERE      (THANA_CODE = a.mail_thana_code)) AS mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, 
                      cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, 
                      mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, 
                      child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, guard_photo4, entry_user, entry_date, update_user, 
                      update_date, religion, std_cur_photo, (SELECT     class_name
                            FROM    class_info  WHERE      (class_id = (SELECT     class_id FROM          std_current_status AS b  WHERE      (a.ID = student_id)))) AS stcs_class, (SELECT     class_year FROM   std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_year, (SELECT shift FROM          std_current_status AS b WHERE(a.ID = student_id)) AS stcs_shift, (SELECT sec_name FROM          section_info  WHERE      (sec_id = (SELECT     sect FROM          std_current_status AS b WHERE      (a.ID = student_id)))) AS stcs_section, (SELECT     class_start FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_class_start, (SELECT     std_roll FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_roll, (SELECT     std_admission_date FROM  std_current_status AS b WHERE (a.ID = student_id)) AS stcs_admission_date
FROM student_info AS a WHERE religion='" + Religion + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable getStudentDistrictAndThanaWiseReport(string District, string Thana)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc,
                          (SELECT DISTRICT_NAME FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) AS per_dist_code,(SELECT     THANA_NAME FROM   THANA_CODE WHERE      (THANA_CODE = a.per_thana_code)) AS per_thana_code, per_zip_code, mail_loc, (SELECT     DISTRICT_NAME FROM          DISTRICT_CODE AS DISTRICT_CODE_1 WHERE   (DISTRICT_CODE = a.mail_dist_code)) AS mail_dist_code, (SELECT     THANA_NAME FROM          THANA_CODE AS THANA_CODE_1 WHERE      (THANA_CODE = a.mail_thana_code)) AS mail_thana_code, mail_zip_code, email, tel_no, mobile_no, nationality, passport, visa_type, visa_exp_date, 
                      cont_person, cont_relate, cont_phone, cont_mobile, cont_address, fth_name, fth_edu, fth_occup, fth_org, fth_tel, fth_oth_act, mth_name, mth_edu, mth_occup, 
                      mth_org, mth_tel, mth_oth_act, prev_sch, prev_add, last_class, class_year, class_pos, reason_leave, physic_prob, allergic, child_rcv1, rel_rcv1, child_rcv2, rel_rcv2, 
                      child_rcv3, rel_rcv3, child_rcv4, rel_rcv4, fth_photo, mth_photo, guard_photo1, guard_photo2, guard_photo3, guard_photo4, entry_user, entry_date, update_user, 
                      update_date, religion, std_cur_photo, (SELECT     class_name
                            FROM    class_info  WHERE      (class_id = (SELECT     class_id FROM          std_current_status AS b  WHERE      (a.ID = student_id)))) AS stcs_class, (SELECT     class_year FROM   std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_year, (SELECT shift FROM          std_current_status AS b WHERE(a.ID = student_id)) AS stcs_shift, (SELECT sec_name FROM          section_info  WHERE      (sec_id = (SELECT     sect FROM          std_current_status AS b WHERE      (a.ID = student_id)))) AS stcs_section, (SELECT     class_start FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_class_start, (SELECT     std_roll FROM          std_current_status AS b WHERE      (a.ID = student_id)) AS stcs_roll, (SELECT     std_admission_date FROM  std_current_status AS b WHERE (a.ID = student_id)) AS stcs_admission_date
FROM student_info AS a Where (SELECT DISTRICT_CODE FROM   DISTRICT_CODE WHERE (DISTRICT_CODE = a.per_dist_code)) ='" + District + "' AND (SELECT THANA_CODE FROM   THANA_CODE WHERE (THANA_CODE = a.per_thana_code))='" + Thana + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        //*****************************  get Check Duplicate StudentId *******************//

        public static DataTable getCheckDuplicateStudentId(string StudentId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT student_id,f_name+''+m_name+''+l_name as Name, birth_dt, sex, std_photo, per_loc from student_info WHERE  student_id = '" + StudentId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        public static DataTable GetAllStudentSectionWiseRollInformation(string classId, string Verson, string Sec, string Shift)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
//                string Query = @"SELECT b.[student_id] as [Id]
//      ,([f_name]+' '+[m_name]+' '+[l_name]  )as [Name]
//      ,[fth_name] as [Father]
//      ,[mth_name] as [Mother]
//      ,c.class_name as [CLASS NAME]
//      ,s.sec_name as [Section]  
//      ,a.std_roll as [STUDENT ROLL] 
//  FROM [student_info] b 
//  INNER JOIN std_current_status a on a.student_id=b.student_id
//  inner join class_info c on c.class_id=a.class_id
//  inner join section_info s on s.id=a.sect
                //  where a.class_id='" + classId + "' and a.[version]='" + Verson + "' and a.sect='" + Sec + "' and a.shift='" + Shift + "' order by CONVERT(int, a.std_roll) asc ";
                string Query = @"SELECT b.[student_id] as [Id]
      ,([f_name]+' '+[m_name]+' '+[l_name]  )as [Name]
      ,[fth_name] as [Father]
      ,[mth_name] as [Mother]
      ,c.class_name as [CLASS NAME]
      ,s.sec_name as [Section]  
      ,a.std_roll as [STUDENT ROLL] 
      ,v.version_name
  FROM [student_info] b 
  INNER JOIN std_current_status a on a.student_id=b.student_id
  inner join class_info c on c.class_id=a.class_id
  inner join section_info s on s.sec_id=a.sect  
  inner join version_info v on v.id=a.[version]
  where a.class_id='" + classId + "' and a.[version]='" + Verson + "' and a.sect='" + Sec + "' and a.shift='" + Shift + "' order by CONVERT(int, a.std_roll) asc ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable getStds(string ClassId, string RollId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT [student_id] as [Id]
      ,([f_name]+' '+[m_name]+' '+[l_name]  )as [Name]
      ,[fth_name] as [Father]
      ,[mth_name] as [Mother]
      ,(select class_name from class_info where class_id=(SELECT [class_id] FROM [std_current_status] WHERE [student_id]=b.[student_id]))AS [CLASS NAME]
      ,(select sec_name from section_info where sec_id=(SELECT [sect]  FROM [std_current_status] WHERE [student_id]=b.[student_id]))As [Section]  
,(SELECT [std_roll]  FROM [std_current_status] WHERE [student_id]=b.[student_id])AS [STUDENT ROLL] 
  FROM [student_info] b where (SELECT [class_id] FROM [std_current_status] a WHERE a.[student_id]=b.[student_id])='" + ClassId + "' and (SELECT [sect]  FROM [std_current_status] a WHERE a.[student_id]=b.[student_id])='" + RollId + "' ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        public static DataTable getShowTeacherName(string TeacherId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT [emp_employee_id]      
      ,([emp_first_name]+' '+[emp_middle_name]+' '+[emp_last_name]) as[employee_name]        
  FROM [tbl_employee_information] where [emp_employee_id]='" + TeacherId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        //****************************** Report On i Text *************************//
        public static DataTable GetShowReportOnStudentInfo()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT t1.[student_id],(t1.[f_name]+' '+t1.[m_name]+' '+t1.[l_name])student_name,convert(varchar,t1.[birth_dt],103) as [birth_dt],t1.[per_loc],t1.[mobile_no],t1.[fth_name],t1.[fth_occup] ,t1.[mth_name]      
      ,t1.[mth_occup],case
      when t1.[religion]='I' then 'Islam'
      when t1.[religion]='C' then 'Christian'
      when t1.[religion]='H' then 'Hindu'
      when t1.[religion]='B' then 'Buddha'
      ELSE ''
      end as[Religion],case 
      when t1.sex='F' then 'Femail'
      when t1.sex='M' then 'Male'
      ELSE ''
	  END as[Sex],t2.std_roll,t3.class_name,t4.sec_name,convert(varchar,t2.std_admission_date,103) as std_admission_date FROM [student_info] t1
  inner join std_current_status t2 on t2.student_id=t1.student_id and t1.[status]='1'
  inner join class_info t3 on t3.class_id=t2.class_id
  inner join section_info t4 on t4.sec_id=t2.sect order by t1.[student_id]";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetShowReportOnStudentInfoOnClass(string ClassId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT t1.[student_id],(t1.[f_name]+' '+t1.[m_name]+' '+t1.[l_name])student_name,convert(varchar,t1.[birth_dt],103) as [birth_dt],t1.[per_loc],t1.[mobile_no],t1.[fth_name]
,case
      when t1.[fth_occup]='E' then 'Engineer'
      when t1.[fth_occup]='D' then 'Doctor'
      when t1.[fth_occup]='L' then 'Lawyer'
      when t1.[fth_occup]='P' then 'Private Service'
      when t1.[fth_occup]='G' then 'Govt.Service'
      when t1.[fth_occup]='T' then 'Teaching'
      when t1.[fth_occup]='B' then 'Business'
      when t1.[fth_occup]='O' then 'Others'
      ELSE '' end as [fth_occup]
 ,t1.[mth_name]  
      , case
      when t1.[mth_occup]='E' then 'Engineer'
      when t1.[mth_occup]='D' then 'Doctor'
      when t1.[mth_occup]='L' then 'Lawyer'
      when t1.[mth_occup]='P' then 'Private Service'
      when t1.[mth_occup]='G' then 'Govt.Service'
      when t1.[mth_occup]='T' then 'Teaching'
      when t1.[mth_occup]='B' then 'Business'
      when t1.[mth_occup]='O' then 'Others'
      ELSE '' end as [mth_occup]      
      ,case
      when t1.[religion]='I' then 'Islam'
      when t1.[religion]='C' then 'Christian'
      when t1.[religion]='H' then 'Hindu'
      when t1.[religion]='B' then 'Buddha'
      ELSE ''
      end as[Religion],case 
      when t1.sex='F' then 'Femail'
      when t1.sex='M' then 'Male'
      ELSE ''
	  END as[Sex],t2.std_roll,t3.class_name,t4.sec_name,convert(varchar,t2.std_admission_date,103) as std_admission_date FROM [student_info] t1
  inner join std_current_status t2 on t2.student_id=t1.student_id
  inner join class_info t3 on t3.class_id=t2.class_id and t2.class_id='" + ClassId + "' inner join section_info t4 on t4.sec_id=t2.sect order by CONVERT(int,t2.std_roll) asc";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetShowReportOnStudentInfoClassAndSec(string ClassId, string SectionId,string Version,string ShiftId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Condition = "";
            if (ClassId != "" && Version == "" && ShiftId == "" && SectionId == "") { Condition = "where b.class_id='" + ClassId + "' order by CONVERT(int,b.std_roll)"; }
            else if (ClassId != "" && ShiftId != "" && SectionId == "" && Version == "") { Condition = "where b.class_id='" + ClassId + "' AND b.shift='" + ShiftId + "' order by CONVERT(int,b.std_roll)"; }
            else if (ClassId != "" && Version != "" && SectionId == "" && ShiftId == "") { Condition = "where b.class_id='" + ClassId + "' and b.[version]='" + Version + "' order by CONVERT(int,b.std_roll)"; }
            else if (ClassId != "" && Version != "" && ShiftId != "" && SectionId == "") { Condition = "where b.class_id='" + ClassId + "' and b.[version]='" + Version + "' and b.shift='" + ShiftId + "' order by CONVERT(int,b.std_roll)"; }
            else if (ClassId != "" && Version != "" && ShiftId != "" && SectionId != "") { Condition = "where b.class_id='" + ClassId + "' and b.[version]='" + Version + "' and b.shift='" + ShiftId + "'  and b.sect='" + SectionId + "' order by CONVERT(int,b.std_roll)"; }
            
            try
            {
                myConnection.Open();
                string Query = @"Select b.std_roll, a.student_id,c.class_name,sc.sec_name, a.f_name,a.fth_name, s.shift_name,vi.version_name,a.mth_name,a.mobile_no,a.blood_group From student_info a
inner join std_current_status b on b.student_id=a.ID
inner join shift_info s on s.shift_id=b.shift  
inner join version_info vi on vi.version_id=b.[version]
inner join class_info c on c.class_id=b.class_id inner join section_info sc on sc.sec_id=b.sect " + Condition;
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetShowReportOnStudentInfoOnArchiveReport(string ClassId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT t1.[student_id],(t1.[f_name]+' '+t1.[m_name]+' '+t1.[l_name])student_name,convert(varchar,t1.[birth_dt],103) as [birth_dt],t1.[per_loc],t1.[mobile_no],t1.[fth_name]
,case
      when t1.[fth_occup]='E' then 'Engineer'
      when t1.[fth_occup]='D' then 'Doctor'
      when t1.[fth_occup]='L' then 'Lawyer'
      when t1.[fth_occup]='P' then 'Private Service'
      when t1.[fth_occup]='G' then 'Govt.Service'
      when t1.[fth_occup]='T' then 'Teaching'
      when t1.[fth_occup]='B' then 'Business'
      when t1.[fth_occup]='O' then 'Others'
      ELSE '' end as [fth_occup]
 ,t1.[mth_name]  
      , case
      when t1.[mth_occup]='E' then 'Engineer'
      when t1.[mth_occup]='D' then 'Doctor'
      when t1.[mth_occup]='L' then 'Lawyer'
      when t1.[mth_occup]='P' then 'Private Service'
      when t1.[mth_occup]='G' then 'Govt.Service'
      when t1.[mth_occup]='T' then 'Teaching'
      when t1.[mth_occup]='B' then 'Business'
      when t1.[mth_occup]='O' then 'Others'
      ELSE '' end as [mth_occup]      
      ,case
      when t1.[religion]='I' then 'Islam'
      when t1.[religion]='C' then 'Christian'
      when t1.[religion]='H' then 'Hindu'
      when t1.[religion]='B' then 'Buddha'
      ELSE ''
      end as[Religion],case 
      when t1.sex='F' then 'Femail'
      when t1.sex='M' then 'Male'
      ELSE ''
	  END as[Sex],t2.std_roll,t3.class_name,t4.sec_name,convert(varchar,t2.std_admission_date,103) as std_admission_date FROM [student_info] t1
  inner join std_current_status t2 on t2.student_id=t1.student_id and t1.[status]='1'
  inner join class_info t3 on t3.class_id=t2.class_id and t2.class_id='" + ClassId + "' inner join section_info t4 on t4.sec_id=t2.sect order by CONVERT(int, t2.std_roll) asc";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable getScetionName()
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select class_id,class_name from class_info";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "StudentInfo");
            return dt;
        }

        public static DataTable getAllStudentInfo(string ClassID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT     CONVERT(int, T1a.class_id) AS ClassId, CONVERT(int, T1a.sect) AS SectID, CONVERT(int, T1a.std_roll) AS Roll, t1c.class_name, t1s.sec_name, T1a.student_id, T1a.class_year, dbo.InitCap(T1.f_name + ' ' + T1.m_name + ' ' + T1.l_name) AS StudentName
FROM         dbo.student_info AS T1 INNER JOIN dbo.std_current_status AS T1a ON T1.student_id = T1a.student_id AND T1.status = 1 INNER JOIN dbo.class_info AS t1c ON T1a.class_id = t1c.class_id and T1a.class_id='" + ClassID + "' INNER JOIN dbo.section_info AS t1s ON T1a.sect = t1s.sec_id GROUP BY CONVERT(int, T1a.class_id), CONVERT(int, T1a.sect), CONVERT(int, T1a.std_roll), t1c.class_name, t1s.sec_name, T1a.student_id, T1a.class_year, dbo.InitCap(T1.f_name + ' ' + T1.m_name + ' ' + T1.l_name)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "StudentInfo");
            return dt;
        }

        public static DataTable getStudentCountClass(string ClassID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"Select count(*) TotalStd from  std_current_status Where class_id='" + ClassID + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "StudentCount");
            return dt;
        }

        public static DataTable getStudentCountClassSect(string ClassID, string Section)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"Select count(*) TotalStd from  std_current_status Where class_id='" + ClassID + "' and  sect='" + Section + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "StudentCount");
            return dt;
        }

        public static DataTable getStudentCount(string ClassId, string SectionId, string Version, string ShiftId)
        {
            String connectionString = DataManager.OraConnString();

            string Condition = "";
            if (ClassId != "" && Version == "" && ShiftId == "" && SectionId == "") { Condition = "where t2.class_id='" + ClassId + "' "; }
            else if (ClassId != "" && ShiftId != "" && SectionId == "" && Version == "") { Condition = "where t2.class_id='" + ClassId + "' AND t2.shift='" + ShiftId + "' "; }
            else if (ClassId != "" && Version != "" && SectionId == "" && ShiftId == "") { Condition = "where t2.class_id='" + ClassId + "' and t2.[version]='" + Version + "' "; }
            else if (ClassId != "" && Version != "" && ShiftId != "" && SectionId == "") { Condition = "where t2.class_id='" + ClassId + "' and t2.[version]='" + Version + "' and t2.shift='" + ShiftId + "'"; }
            else if (ClassId != "" && Version != "" && ShiftId != "" && SectionId != "") { Condition = "where t2.class_id='" + ClassId + "' and t2.[version]='" + Version + "' and t2.shift='" + ShiftId + "'  and t2.sect='" + SectionId + "' "; }

            string query = @"Select count(*) TotalStd from  std_current_status t2 "+Condition;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "StudentCount");

            return dt;
        }

        public static DataTable getStudentCountClassAndSec(string ClassId, string SectionId)
        {
            String connectionString = DataManager.OraConnString();

            string Condition = "";
            if (ClassId != ""  && SectionId == "") { Condition = "where t2.class_id='" + ClassId + "' "; }        
            
            
            else if (ClassId != "" && SectionId != "") { Condition = "where t2.class_id='" + ClassId + "'  and t2.sect='" + SectionId + "' "; }

            string query = @"Select count(*) TotalStd from  std_current_status t2 " + Condition;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "StudentCount");

            return dt;
        }

        public static DataTable GetShowReportOnStudentInfoOnArchiveReport()
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT t1.[student_id],(t1.[f_name]+' '+t1.[m_name]+' '+t1.[l_name])student_name,convert(varchar,t1.[birth_dt],103) as [birth_dt],t1.[per_loc],t1.[mobile_no],t1.[fth_name]
,case
      when t1.[fth_occup]='E' then 'Engineer'
      when t1.[fth_occup]='D' then 'Doctor'
      when t1.[fth_occup]='L' then 'Lawyer'
      when t1.[fth_occup]='P' then 'Private Service'
      when t1.[fth_occup]='G' then 'Govt.Service'
      when t1.[fth_occup]='T' then 'Teaching'
      when t1.[fth_occup]='B' then 'Business'
      when t1.[fth_occup]='O' then 'Others'
      ELSE '' end as [fth_occup]
 ,t1.[mth_name]  
      , case
      when t1.[mth_occup]='E' then 'Engineer'
      when t1.[mth_occup]='D' then 'Doctor'
      when t1.[mth_occup]='L' then 'Lawyer'
      when t1.[mth_occup]='P' then 'Private Service'
      when t1.[mth_occup]='G' then 'Govt.Service'
      when t1.[mth_occup]='T' then 'Teaching'
      when t1.[mth_occup]='B' then 'Business'
      when t1.[mth_occup]='O' then 'Others'
      ELSE '' end as [mth_occup]      
      ,case
      when t1.[religion]='I' then 'Islam'
      when t1.[religion]='C' then 'Christian'
      when t1.[religion]='H' then 'Hindu'
      when t1.[religion]='B' then 'Buddha'
      ELSE ''
      end as[Religion],case 
      when t1.sex='F' then 'Femail'
      when t1.sex='M' then 'Male'
      ELSE ''
	  END as[Sex],t2.std_roll,t3.class_name,t4.sec_name,convert(varchar,t2.std_admission_date,103) as std_admission_date FROM [student_info] t1
  inner join std_current_status t2 on t2.student_id=t1.student_id and t1.[status]='1'
  inner join class_info t3 on t3.class_id=t2.class_id 
  inner join section_info t4 on t4.sec_id=t2.sect order by t1.[student_id]";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }


        //***************************** Show On History ***************************************//

        public static DataTable GetShowPaymentInformation(string StId, string Class, string Year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"SELECT t1.[PAYMENT_ID]
      ,t1.[PAY_DATE]
      ,t1.[PAY_MODE]
      ,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) std_name
      ,t3.class_name     
      ,t4.sec_name
      ,t1.[PAY_YEAR]          
      ,t1.[REF_NO]
  FROM [PAYMENT_MST] t1  inner join student_info t2 on t2.student_id=t1.STUDENT_ID and t1.STUDENT_ID='" + StId + "'  inner join class_info t3 on t3.class_id=t1.class_id and t1.class_id='" + Class + "' inner join section_info t4 on t4.sec_id=t1.sect where t1.[PAY_YEAR]='" + Year + "' ";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }
        public static DataTable GetShowAllStudentIdCardReceivedInformation(string Std_Id, string Class, string Section, string Verson, string Shift,string flag)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string Condition = "";
            string TabelField = "";
            if (flag == "Print") //Id Card Print History
            {
                if (Std_Id != "" && Class == "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where t1.[student_id]='" + Std_Id + "' and isnull(t1.Idcard_Print,0)='1'  order by convert(int,b.std_roll)"; }

                else if (Std_Id != "" && Class != "" && Section != "" && Shift != "" && Verson != "") { Condition = "Where t1.[student_id]='" + Std_Id + "' and isnull(t1.Idcard_Print,0)='1' order by convert(int,b.std_roll) "; }

                else if (Std_Id != "" && Class != "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where t1.[student_id]='" + Std_Id + "' And b.class_id='" + Class + "' and isnull(t1.Idcard_Print,0)='1' order by convert(int,b.std_roll) "; }

                else if (Std_Id == "" && Class != "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where b.class_id='" + Class + "' and isnull(t1.Idcard_Print,0)='1' order by convert(int,b.std_roll) "; }

                else if (Std_Id == "" && Class != "" && Section != "" && Shift != "" && Verson != "") { Condition = "Where b.class_id='" + Class + "' and b.sect='" + Section + "' and b.shift='" + Shift + "' and b.version='" + Verson + "' and isnull(t1.Idcard_Print,0)='1' order by convert(int,b.std_roll) "; }
                
                TabelField = "isnull(t1.Idcard_Print,0)Id_Card_falg";
            }
            else //Id Card Receive and Delevery
            {
                if (Std_Id != "" && Class == "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where t1.[student_id]='" + Std_Id + "' and isnull(t1.Id_Card_falg,0)=" + flag + "  order by convert(int,b.std_roll)"; }

                else if (Std_Id != "" && Class != "" && Section != "" && Shift != "" && Verson != "") { Condition = "Where t1.[student_id]='" + Std_Id + "' and isnull(t1.Id_Card_falg,0)=" + flag + " order by convert(int,b.std_roll) "; }

                else if (Std_Id != "" && Class != "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where t1.[student_id]='" + Std_Id + "' And b.class_id='" + Class + "' and isnull(t1.Id_Card_falg,0)=" + flag + " order by convert(int,b.std_roll) "; }

                else if (Std_Id == "" && Class != "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where b.class_id='" + Class + "' and isnull(t1.Id_Card_falg,0)=" + flag + " order by convert(int,b.std_roll) "; }

                else if (Std_Id == "" && Class != "" && Section != "" && Shift != "" && Verson != "") { Condition = "Where b.class_id='" + Class + "' and b.sect='" + Section + "' and b.shift='" + Shift + "' and b.version='" + Verson + "' and isnull(t1.Id_Card_falg,0)=" + flag + " order by convert(int,b.std_roll) "; }
                TabelField = "isnull(t1.Id_Card_falg,0)Id_Card_falg";
            } //else { Condition = ""; }

            string query = @"SELECT t1.[student_id] AS[Std_Id],t1.[f_name]+' '+t1.m_name+' '+t1.l_name AS[Std_name],t1.fth_name AS[Father_Name],t1.mth_name AS[Mother_Name],c.class_name as[Class],sc.sec_name as[Section], sh.shift_name as[Shift],vv.version_name,b.std_roll as[Roll],t1.mobile_no as[Mobil_No]," + TabelField + ",convert(nvarchar,t1.Id_Card_Rcv_Date,103)Id_Card_Rcv_Date FROM [student_info] t1 inner join std_current_status b on t1.student_id=b.student_id inner join class_info c on c.class_id=b.class_id left join section_info sc on sc.sec_id=b.sect inner join shift_info sh on sh.shift_id=b.shift left join version_info vv on vv.id=b.[version] " + Condition;

            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
            return dt;
        }

        public static void getIdCardInformation(string Std_Id,string Falg)
        {
            String connectionString = DataManager.OraConnString();
            string query = "";
            if (Falg == "Print")
            {
                query = @"UPDATE [student_info] SET [Idcard_Print] ='0' WHERE [student_id]='" + Std_Id + "'";
            }
            else
            {
                if (Falg == "0") { query = @"UPDATE [student_info] SET [Id_Card_falg] ='" + Falg + "',[Id_Card_Rcv_Date] =null WHERE [student_id]='" + Std_Id + "'"; }
                else { query = @"UPDATE [student_info] SET [Id_Card_falg] ='" + Falg + "',[Id_Card_Rcv_Date] =GETDATE() WHERE [student_id]='" + Std_Id + "'"; }
            }
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void GetIdCardPrintInfo(string Std_Id)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"UPDATE [student_info] SET [Idcard_Print] ='1' WHERE [student_id]='" + Std_Id + "'"; 
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static object GetShowSectionOnShift(string classId, string versionId, string shiftId)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"select sec.sec_id,sec.sec_name from dbo.class_info ci inner join version_info vi on ci.class_id=vi.class_id inner join shift_info si on si.class_id=ci.class_id inner join section_info sec on sec.class_id=ci.class_id
  where ci.class_id='" + classId + "' and vi.version_id='" + versionId + "' and si.shift_id='" + shiftId + "'";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "section_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
        }

        public static DataTable GetStudentInfoMarkEntryByGroupBYSubject(string classid, string versionid, string shiftid, string sectid, string groupid, string year, string Examid, string subid)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                string Query = @"  
 Select Cs.student_id,([f_name]+' '+[m_name]+' '+[l_name])as [Name],cs.std_roll, ri.objective_Mark as objective_Mark,ri.theory_marks as theory_marks,ri.practical_marks as practical_marks, ri.MT as MT,ri.grade
from std_current_status cs inner join dbo.Subject_Choice_Mst sm on cs.student_id=sm.student_id inner join dbo.Subject_Choice_Dtls sd on sm.id=sd.mst_id and  sd.sub_id='" + subid + "' and  sm.group_id='" + groupid + "'" +

"inner join student_info si on cs.student_id=si.student_id and cs.class_id='" + classid + "' and cs.version='" + versionid + "' and cs.shift='" + shiftid + "' and cs.sect='" + sectid + "' and cs.group_name='" + groupid + "' and cs.class_year='" + year + "' " +
" left join tbl_Result_info ri  on cs.student_id =ri.student_id and cs.class_year=ri.class_year and cs.class_id=ri.class_id and ri.exam_title_id='" + Examid + "' and ri.sub_id='" + subid + "'" +
" Order by CONVERT(int,cs.std_roll ) asc";
                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }

        }

        public static DataTable GetStudentInfoMarkEntryNewBysubject(string classid, string versionid, string shiftid, string sectid, string year, string Examid, string subid, int addFlag, string Group)
        {

            using (SqlConnection conn = new SqlConnection(DataManager.OraConnString()))
            {
                SqlCommand sqlComm = new SqlCommand("SP_ResultInfoEntryDetails", conn);
                sqlComm.Parameters.AddWithValue("@ClassId", classid);
                sqlComm.Parameters.AddWithValue("@versionId", versionid);
                sqlComm.Parameters.AddWithValue("@shiftId", shiftid);
                sqlComm.Parameters.AddWithValue("@sectId", sectid);
                sqlComm.Parameters.AddWithValue("@yearID", year);
                sqlComm.Parameters.AddWithValue("@Examid", Examid);
                if (string.IsNullOrEmpty(Group))
                {
                    sqlComm.Parameters.AddWithValue("@GroupID", null);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@GroupID", Group);
                }
                sqlComm.Parameters.AddWithValue("@SubjectID", subid);
                sqlComm.Parameters.AddWithValue("@Flag", addFlag);
            

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = sqlComm;
                da.Fill(ds, "SP_ResultInfoEntryDetails");
                DataTable dtRetak = ds.Tables["SP_ResultInfoEntryDetails"];
                return dtRetak;
            }

//            String ConnectionString = DataManager.OraConnString();
//            SqlConnection myConnection = new SqlConnection(ConnectionString);
//            try
//            {
//                myConnection.Open();
//                string Query = "";
//                if (addFlag == 0)
//                {
//                    Query = @"Select si.ID,si.student_id,([f_name]+' '+[m_name]+' '+[l_name])as [Name],cs.std_roll,CASE WHEN (ri.Org_ObjMark IS Null OR ri.Org_ObjMark=0) then ri.objective_Mark else ri.Org_ObjMark end as objective_Mark,CASE WHEN (ri.Org_ThMark IS NULL OR ri.Org_ThMark=0) then ri.theory_marks else ri.Org_ThMark end as theory_marks,ri.practical_marks as practical_marks, ri.MT as MT,ri.grade
//from std_current_status cs
//inner join student_info si on cs.student_id=si.ID and cs.class_id='" + classid.Trim() + "' and cs.version='" + versionid.Trim() + "' and cs.shift='" + shiftid.Trim() + "' and cs.sect='" + sectid.Trim() + "' and cs.class_year='" + year.Trim() + "' and cs.group_name='"+Group+"' " +
//    "  and ((Select sub_optional from subject_info where subject_id='" + subid.Trim() + "') ='A' or si.sex = (Select sub_optional from subject_info where subject_id='" + subid.Trim() + "')) left join tbl_Result_info ri  on cs.student_id =ri.student_id and cs.class_year=ri.class_year and cs.class_id=ri.class_id and ri.exam_title_id='" + Examid.Trim() + "' and ri.sub_id='" + subid.Trim() + "'" +
//    " Order by CONVERT(int,cs.std_roll)  asc";
//                }
//                else if (addFlag == 1)
//                {
//                    string Pra = "";
//                    if (Group != "") { Pra = "and cs.group_name='" + Group + "'"; }
//                    Query = @"Select si.ID,si.student_id,([f_name]+' '+[m_name]+' '+[l_name])as [Name],cs.std_roll,CASE WHEN (ri.Org_ObjMark IS Null OR ri.Org_ObjMark=0) then ri.objective_Mark else ri.Org_ObjMark end as objective_Mark,CASE WHEN (ri.Org_ThMark IS NULL OR ri.Org_ThMark=0) then ri.theory_marks else ri.Org_ThMark end as theory_marks,ri.practical_marks as practical_marks, ri.MT as MT,ri.grade
//from std_current_status cs
//inner join student_info si on cs.student_id=si.ID and cs.class_id='" + classid.Trim() + "' and cs.[version]='" + versionid.Trim() + "' and cs.shift='" + shiftid.Trim() + "' and cs.sect='" + sectid.Trim() + "' and cs.class_year='" + year.Trim() + "' inner join Subject_Choice_Mst t1 on t1.student_id=cs.student_id inner join Subject_Choice_Dtls t2 on t2.mst_id=t1.id  and t2.sub_id='" + subid.Trim() + "'  " + Pra + " left join tbl_Result_info ri  on cs.student_id =ri.student_id and cs.class_year=ri.class_year and cs.class_id=ri.class_id and ri.exam_title_id='" + Examid.Trim() + "' and ri.sub_id='" + subid.Trim() + "' Order by CONVERT(int,cs.std_roll)  asc";
//                }
//                DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "student_info");
//                return dt;

//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (myConnection.State == ConnectionState.Open)
//                    myConnection.Close();
//            }

        }
        public static DataTable GetAllStudentSearch(string ClassID, string SectionID, string GroupID, string ShiftID, string StudentId, string Type)
        {
            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            {
                SqlCommand sqlComm = new SqlCommand("[SP_StudentInformationSearch]", conn);

                if (string.IsNullOrEmpty(StudentId))
                {
                    sqlComm.Parameters.AddWithValue("@StudentId", null);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@StudentID", StudentId);
                }
                //**** Class 
                if (!string.IsNullOrEmpty(ClassID))
                {
                    sqlComm.Parameters.AddWithValue("@ClassID", ClassID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@ClassID", null);
                }
                // ***** SectionID
                if (!string.IsNullOrEmpty(SectionID))
                {
                    sqlComm.Parameters.AddWithValue("@SectionID", SectionID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@SectionID", null);
                }
                //********* VersionID
                sqlComm.Parameters.AddWithValue("@version", null);
                //*** Shift
                if (!string.IsNullOrEmpty(ShiftID))
                {
                    sqlComm.Parameters.AddWithValue("@ShiftID", ShiftID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@ShiftID", null);
                }

                sqlComm.Parameters.AddWithValue("@classyear", null);



                //*** GroupID
                if (!string.IsNullOrEmpty(GroupID))
                {
                    sqlComm.Parameters.AddWithValue("@GroupID", GroupID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@GroupID", null);
                }
                sqlComm.Parameters.AddWithValue("@Roll", null);
                sqlComm.Parameters.AddWithValue("@Religion", null);
                sqlComm.Parameters.AddWithValue("@StudentStatus", null);
                sqlComm.Parameters.AddWithValue("@Name", null);
                if (!string.IsNullOrEmpty(Type))
                {
                    sqlComm.Parameters.AddWithValue("@Type", Type);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Type", null);
                }

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = sqlComm;
                da.Fill(ds, "SP_StudentformationDetails");
                DataTable dtStdDtl = ds.Tables["SP_StudentformationDetails"];
                return dtStdDtl;
            }
        }

        public static string GetStudentID(string StudentID)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            try
            {
                string query = @"select ID from STUDENT_INFO where STUDENT_ID='" + StudentID + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                //object ID=cmd.ExecuteScalar().ToString();
                //if (ID==null)
                //    return "";
                //else
                //return (string)ID;

                var getValue = cmd.ExecuteScalar();
                return (getValue == null) ? string.Empty : getValue.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                connection.Close();
            }
        }

        public static DataTable GetAllStudentSearchByDept(string DeptID, string DeptTypeID, string SessionID, string Batch, string StudentID, string CampusID, string Year, string FromDate, string ToDate)
        {
            using (SqlConnection conn = new SqlConnection(DataManager.OraConnString()))
            {
                SqlCommand sqlComm = new SqlCommand("SP_StudentInformationDisplay", conn);
                if (!string.IsNullOrEmpty(DeptID))
                {
                    sqlComm.Parameters.AddWithValue("@DepartmentID", DeptID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@DepartmentID", null);
                }
                if (!string.IsNullOrEmpty(DeptTypeID))
                {
                    sqlComm.Parameters.AddWithValue("@DepartmentTypeID", DeptTypeID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@DepartmentTypeID", null);
                }
                if (!string.IsNullOrEmpty(SessionID))
                {
                    sqlComm.Parameters.AddWithValue("@SessionID", SessionID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@SessionID", null);
                }
                if (!string.IsNullOrEmpty(Batch))
                {
                    sqlComm.Parameters.AddWithValue("@Batch", Batch);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Batch", null);
                }
                if (!string.IsNullOrEmpty(Year))
                {
                    sqlComm.Parameters.AddWithValue("@Year", Year);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Year", null);
                }
                if (!string.IsNullOrEmpty(CampusID))
                {
                    sqlComm.Parameters.AddWithValue("@CampusID", CampusID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@CampusID", null);
                }
                if (!string.IsNullOrEmpty(StudentID))
                {
                    sqlComm.Parameters.AddWithValue("@StudentID", StudentID);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@StudentID", null);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    sqlComm.Parameters.AddWithValue("@FromDate", FromDate);
                    sqlComm.Parameters.AddWithValue("@ToDate", ToDate);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@FromDate", null);
                    sqlComm.Parameters.AddWithValue("@ToDate", null);
                }
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = sqlComm;
                da.Fill(ds, "SP_StudentInformationDisplay");
                DataTable dtStdInfo = ds.Tables["SP_StudentInformationDisplay"];
                return dtStdInfo;
            }
        }
    }
}