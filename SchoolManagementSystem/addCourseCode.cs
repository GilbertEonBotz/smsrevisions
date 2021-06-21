﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EonBotzLibrary;
using SqlKata.Execution;

namespace SchoolManagementSystem
{
    public partial class addCourseCode : Form
    {
        CourseCode reloadDatagrid;
        public addCourseCode(CourseCode reloadDatagrid)
        {
            InitializeComponent();
            this.reloadDatagrid = reloadDatagrid;

        }

        int selCourseID;
        private void btnSave_Click(object sender, EventArgs e)
        {
            ComboBox[] cmb = { cmbDepartment };

            if (btnSave.Text.Equals("Update"))
            {
                if (Validator.isEmptyCmb(cmb))
                {
                    if (Validator.UpdateConfirmation())
                    {
                        try
                        {
                            DBContext.GetContext().Query("coursecode").Where("coursecode", txtCourseCode.Text).First();
                            Validator.AlertDanger("Course code already existed!");
                        }
                        catch (Exception)
                        {
                            DBContext.GetContext().Query("coursecode").Where("coursecodeId", lblIDD.Text).Update(new
                            {
                                courseId = selCourseID,
                                coursecode = txtCourseCode.Text.ToUpper(),
                                remarks = txtRemarks.Text,
                                status = "enable"
                            });
                            Validator.AlertSuccess("Course code updated");
                            reloadDatagrid.displayData();
                            this.Close();
                        }
                    }

                }
            }
            else if (btnSave.Text.Equals("Save"))
            {
                if (Validator.isEmptyCmb(cmb))
                {
                    try
                    {
                        DBContext.GetContext().Query("coursecode").Where("coursecode", txtCourseCode.Text).First();
                        Validator.AlertDanger("Course code already existed!");
                    }
                    catch (Exception)
                    {
                        DBContext.GetContext().Query("coursecode").Insert(new
                        {
                            courseId = selCourseID,
                            coursecode = txtCourseCode.Text.ToUpper(),
                            remarks = txtRemarks.Text,
                            status = "enable"
                        });
                        Validator.AlertSuccess("Course code inserted");
                        reloadDatagrid.displayData();
                        this.Close();
                    }
                }
                else
                {
                }
            }
        }

        private void addCourseCode_Load(object sender, EventArgs e)
        {
            displayData();
            try
            {
                var value = DBContext.GetContext().Query("course").Where("description", cmbDepartment.Text).First();
                selCourseID = value.courseId;
            }
            catch (Exception)
            {
                selCourseID = 0;
            }

        }

        public void displayData()
        {
            var values = DBContext.GetContext().Query("course").Get();

            foreach (var value in values)
            {
                cmbDepartment.Items.Add(value.description);
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = DBContext.GetContext().Query("course").Where("description", cmbDepartment.Text).First();
            selCourseID = value.courseId;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
