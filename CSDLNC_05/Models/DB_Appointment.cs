﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSDLNC_05.Controllers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace CSDLNC_05.Models
{
    internal class DB_Appointment
    {
        public static List<Appointment>? getAppointmentsByDentistId(int dentistId, DateTime startDate, DateTime endDate)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("get_appointment_report");
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Connection = new DbConn().conn;
                sqlCmd.Parameters.AddWithValue("@user_id", dentistId);
                sqlCmd.Parameters.AddWithValue("@start_date", startDate);
                sqlCmd.Parameters.AddWithValue("@end_date", endDate);
                SqlDataReader res = sqlCmd.ExecuteReader();

                List<Appointment> appointments = new List<Appointment>();

                while (res.Read())
                {
                    Appointment appointment = new Appointment(
                        res.GetDateTime(0),
                        res.GetInt32(1),
                        res.GetString(2),
                        res.GetString(3),
                        res.GetString(4),
                        res.GetInt32(5),
                        res.GetInt32(6),
                        res.GetInt32(7),
                        res.GetInt32(8)
                    );
                    appointments.Add(appointment);
                }
                return appointments;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return null;
            }
        }

        public static List<Appointment>? getAppointmentsByDate (DateTime date, int branchID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("show_appointment_in_date");
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Connection = new DbConn().conn;
                sqlCmd.Parameters.AddWithValue("@branch_id", branchID);
                sqlCmd.Parameters.AddWithValue("@date", date);
                SqlDataReader res = sqlCmd.ExecuteReader();

                List<Appointment> appointments = new List<Appointment>();

                while (res.Read())
                {
                    Appointment appointment = new Appointment(
                        res.GetDateTime(0),
                        res.GetInt32(1),
                        res.GetString(2),
                        res.IsDBNull(3) ? null : res.GetString(3),
                        res.GetString(4),
                        res.GetInt32(5),
                        res.GetInt32(6),
                        res.IsDBNull(7) ? null : res.GetInt32(7),
                        res.IsDBNull(8) ? null : res.GetInt32(8)
                    );

                    appointments.Add(appointment);
                }
                return appointments;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return null;
            }
        }
    
        public static List<Appointment>? filterAppointment (DateTime date, int branch_ID, int dentist, int patient, int clinic)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("filter_appointment");
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Connection = new DbConn().conn;
                sqlCmd.Parameters.AddWithValue("@branch_id", branch_ID);
                sqlCmd.Parameters.AddWithValue("@date", date);
                sqlCmd.Parameters.AddWithValue("@patient_id", patient);
                sqlCmd.Parameters.AddWithValue("@clinic_id", clinic);
                sqlCmd.Parameters.AddWithValue("@dentist_id", dentist);
                SqlDataReader res = sqlCmd.ExecuteReader();

                List<Appointment> appointments = new List<Appointment>();

                while (res.Read())
                {
                    Appointment appointment = new Appointment(
                        res.GetDateTime(0),
                        res.GetInt32(1),
                        res.GetString(2),
                        res.IsDBNull(3) ? null : res.GetString(3),
                        res.GetString(4),
                        res.GetInt32(5),
                        res.GetInt32(6),
                        res.IsDBNull(7) ? null : res.GetInt32(7),
                        res.IsDBNull(8) ? null : res.GetInt32(8)
                    );

                    appointments.Add(appointment);
                }
                return appointments;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return null;
            }
        }

        public static int deleteAppointment(DateTime appointmentDate, int ordinal)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("delete_appointment");
                sqlCmd.Connection = new DbConn().conn;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@appointment_date", appointmentDate);
                sqlCmd.Parameters.AddWithValue("@ordinal", ordinal);
                SqlDataReader res = sqlCmd.ExecuteReader();

                return sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.Print($"Error: {ex.ToString()}");
                return 0;
            }
        }

        public static int getNextOrdinal(DateTime appointmentDate)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("get_next_ordinal");
                sqlCmd.Connection = new DbConn().conn;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@appointment_date", appointmentDate);
                SqlDataReader res = sqlCmd.ExecuteReader();
                int result = 1;
                while (res.Read())
                {
                    result = res.GetInt32(0);
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.Print($"Error: {ex.ToString()}");
                return 1;
            }
        }
    }
}
