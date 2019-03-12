using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Trending
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel1 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel2 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel3 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel4 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel5 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel6 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel7 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.sNUSDataSet = new Trending.SNUSDataSet();
            this.measurementBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.measurementBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.measurementBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.snus_shemaDataSet = new Trending.snus_shemaDataSet();
            this.measurementBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.mySqlGeometryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.resourcesXBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.resourcesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.resourcesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sNUSDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.snus_shemaDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mySqlGeometryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesXBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            customLabel1.FromPosition = -2D;
            customLabel1.Text = "60";
            customLabel1.ToPosition = 2D;
            customLabel2.FromPosition = 8D;
            customLabel2.Text = "50";
            customLabel2.ToPosition = 12D;
            customLabel3.FromPosition = 18D;
            customLabel3.Text = "40";
            customLabel3.ToPosition = 22D;
            customLabel4.FromPosition = 28D;
            customLabel4.Text = "30";
            customLabel4.ToPosition = 32D;
            customLabel5.FromPosition = 38D;
            customLabel5.Text = "20";
            customLabel5.ToPosition = 42D;
            customLabel6.FromPosition = 48D;
            customLabel6.Text = "10";
            customLabel6.ToPosition = 52D;
            customLabel7.FromPosition = 58D;
            customLabel7.Text = "0";
            customLabel7.ToPosition = 62D;
            chartArea1.AxisX.CustomLabels.Add(customLabel1);
            chartArea1.AxisX.CustomLabels.Add(customLabel2);
            chartArea1.AxisX.CustomLabels.Add(customLabel3);
            chartArea1.AxisX.CustomLabels.Add(customLabel4);
            chartArea1.AxisX.CustomLabels.Add(customLabel5);
            chartArea1.AxisX.CustomLabels.Add(customLabel6);
            chartArea1.AxisX.CustomLabels.Add(customLabel7);
            chartArea1.AxisX.Interval = 10D;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX.Title = "Last minute of scanning, shown in seconds (from old to new)";
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Top;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Margin = new System.Windows.Forms.Padding(4);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(1205, 620);
            this.chart.TabIndex = 0;
            this.chart.Text = "Tags and values";
            // 
            // sNUSDataSet
            // 
            this.sNUSDataSet.DataSetName = "SNUSDataSet";
            this.sNUSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // measurementBindingSource
            // 
            this.measurementBindingSource.DataMember = "measurement";
            this.measurementBindingSource.DataSource = this.sNUSDataSet;
            // 
            // measurementBindingSource1
            // 
            this.measurementBindingSource1.DataMember = "measurement";
            this.measurementBindingSource1.DataSource = this.sNUSDataSet;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.dataGridView2.DataSource = this.measurementBindingSource3;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 620);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1205, 159);
            this.dataGridView2.TabIndex = 2;
            // 
            // measurementBindingSource3
            // 
            this.measurementBindingSource3.DataMember = "measurement";
            this.measurementBindingSource3.DataSource = this.snus_shemaDataSet;
            // 
            // snus_shemaDataSet
            // 
            this.snus_shemaDataSet.DataSetName = "snus_shemaDataSet";
            this.snus_shemaDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // measurementBindingSource2
            // 
            this.measurementBindingSource2.DataMember = "measurement";
            this.measurementBindingSource2.DataSource = this.sNUSDataSet;
            // 
            // mySqlGeometryBindingSource
            // 
            this.mySqlGeometryBindingSource.DataSource = typeof(MySql.Data.Types.MySqlGeometry);
            // 
            // resourcesXBindingSource
            // 
            this.resourcesXBindingSource.DataSource = typeof(MySql.Data.ResourcesX);
            // 
            // resourcesBindingSource
            // 
            this.resourcesBindingSource.DataSource = typeof(MySql.Data.Resources);
            // 
            // resourcesBindingSource1
            // 
            this.resourcesBindingSource1.DataSource = typeof(MySql.Data.Resources);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "3";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "4";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "5";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "6";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "7";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "8";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "9";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "10";
            this.Column10.Name = "Column10";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 779);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.chart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Trending";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sNUSDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.snus_shemaDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mySqlGeometryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesXBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private BindingSource measurementBindingSource;
        private SNUSDataSet sNUSDataSet;
        private BindingSource measurementBindingSource1;
        private DataGridView dataGridView2;
        private BindingSource mySqlGeometryBindingSource;
        private BindingSource measurementBindingSource2;
        private BindingSource measurementBindingSource3;
        private snus_shemaDataSet snus_shemaDataSet;
        private BindingSource resourcesXBindingSource;
        private BindingSource resourcesBindingSource;
        private BindingSource resourcesBindingSource1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
    }
}

