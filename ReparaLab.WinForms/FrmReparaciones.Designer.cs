using ReparaLab.Application.UseCases;
using ReparaLab.Domain.Interfaces;

namespace ReparaLab.WinForms
{
    public partial class FrmReparaciones : Form
    {
        private readonly RegistrarOrdenUseCase _registrarUseCase;
        private readonly ListarOrdenesUseCase _listarUseCase;
        private readonly CambiarEstadoOrdenUseCase _cambiarEstadoUseCase;
        private readonly ILogger _logger;

        public FrmReparaciones(
            RegistrarOrdenUseCase registrarUseCase,
            ListarOrdenesUseCase listarUseCase,
            CambiarEstadoOrdenUseCase cambiarEstadoUseCase,
            ILogger logger)
        {
            InitializeComponent();

            // Asignamos las dependencias
            _registrarUseCase = registrarUseCase;
            _listarUseCase = listarUseCase;
            _cambiarEstadoUseCase = cambiarEstadoUseCase;
            _logger = logger;

            // Configuramos la interfaz
            ConfigurarCombos();
            ConfigurarEventos();

            // Cargamos datos iniciales
            _ = CargarTablaAsync();
            ActualizarBitacora();
        }

        private void ConfigurarCombos()
        {
            cmbEquipo.Items.AddRange(new string[] { "LAPTOP", "CELULAR", "TABLET" });
            cmbEquipo.SelectedIndex = 0;

            cmbServicio.Items.AddRange(new string[] { "DIAGNOSTICO", "MANTENIMIENTO" });
            cmbServicio.SelectedIndex = 0;

            cmbPlan.Items.AddRange(new string[] { "BASICO", "PREMIUM" });
            cmbPlan.SelectedIndex = 0;

            cmbNotificacion.Items.AddRange(new string[] { "EMAIL", "SMS" });
            cmbNotificacion.SelectedIndex = 0;
        }
        private void ConfigurarEventos()
        {
            btnRegistrar.Click += BtnRegistrar_Click;
            btnFinalizar.Click += BtnFinalizar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
        }

        private async void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación de UI (Responsabilidad del Formulario)
                if (string.IsNullOrWhiteSpace(txtCliente.Text))
                {
                    MessageBox.Show("El campo Cliente es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCliente.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFalla.Text))
                {
                    MessageBox.Show("Debe describir la falla reportada.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFalla.Focus();
                    return;
                }

                // Delegamos la lógica de negocio al Caso de Uso
                await _registrarUseCase.Ejecutar(
                    cliente: txtCliente.Text.Trim(),
                    equipo: cmbEquipo.Text,
                    falla: txtFalla.Text.Trim(),
                    servicio: cmbServicio.Text,
                    plan: cmbPlan.Text,
                    notificacion: cmbNotificacion.Text,
                    repuesto: chkRepuesto.Checked
                );

                MessageBox.Show("Orden registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await CargarTablaAsync();
                ActualizarBitacora();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Registrar($"ERROR al registrar orden: {ex.Message}");
                ActualizarBitacora();
            }
        }
        private async void BtnFinalizar_Click(object sender, EventArgs e)
        {
            await ProcesarCambioDeEstado("FINALIZAR");
        }

        private async void BtnCancelar_Click(object sender, EventArgs e)
        {
            await ProcesarCambioDeEstado("CANCELAR");
        }

        private async Task ProcesarCambioDeEstado(string accion)
        {
            try
            {
                if (dataGridViewOrdenes.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, seleccione una orden de la tabla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = Convert.ToInt32(dataGridViewOrdenes.CurrentRow.Cells["Id"].Value);

                await _cambiarEstadoUseCase.Ejecutar(id, accion);

                MessageBox.Show($"La orden #{id} ha sido {accion.ToLower()}da.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await CargarTablaAsync();
                ActualizarBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operación no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private async Task CargarTablaAsync()
        {
            var ordenes = await _listarUseCase.Ejecutar();
            dataGridViewOrdenes.DataSource = ordenes.Select(x => new
            {
                x.Id,
                x.Cliente,
                x.Equipo,
                x.Servicio,
                x.Plan,
                Total = x.Total.ToString("0.00"),
                Garantia = x.GarantiaDias,
                x.Estado
            }).ToList();

            if (dataGridViewOrdenes.Columns["Total"] != null) dataGridViewOrdenes.Columns["Total"].HeaderText = "Total ($)";
            if (dataGridViewOrdenes.Columns["Garantia"] != null) dataGridViewOrdenes.Columns["Garantia"].HeaderText = "Garantía (días)";
        }

        private void ActualizarBitacora()
        {
            txtEventos.Clear();
            var eventos = _logger.ObtenerEventos();
            foreach (var evento in eventos)
            {
                txtEventos.AppendText(evento + Environment.NewLine);
            }
            // Auto-scroll al final
            txtEventos.SelectionStart = txtEventos.Text.Length;
            txtEventos.ScrollToCaret();
        }

        private void LimpiarFormulario()
        {
            txtCliente.Clear();
            txtFalla.Clear();
            cmbEquipo.SelectedIndex = 0;
            cmbServicio.SelectedIndex = 0;
            cmbPlan.SelectedIndex = 0;
            cmbNotificacion.SelectedIndex = 0;
            chkRepuesto.Checked = false;
            txtCliente.Focus();
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblCliente = new Label();
            txtCliente = new TextBox();
            lblEquipo = new Label();
            cmbEquipo = new ComboBox();
            lblFallaReportada = new Label();
            txtFalla = new TextBox();
            lblDiagnostico = new Label();
            cmbServicio = new ComboBox();
            lblPlan = new Label();
            cmbPlan = new ComboBox();
            lblNotificaciones = new Label();
            cmbNotificacion = new ComboBox();
            chkRepuesto = new CheckBox();
            btnRegistrar = new Button();
            btnFinalizar = new Button();
            btnCancelar = new Button();
            btnLimpiar = new Button();
            dataGridViewOrdenes = new DataGridView();
            lblEventos = new Label();
            txtEventos = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrdenes).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(442, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reparaciones | Ordenes de Reparaciones";
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(12, 59);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(58, 20);
            lblCliente.TabIndex = 1;
            lblCliente.Text = "Cliente:";
            // 
            // txtCliente
            // 
            txtCliente.Location = new Point(12, 82);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(239, 27);
            txtCliente.TabIndex = 2;
            // 
            // lblEquipo
            // 
            lblEquipo.AutoSize = true;
            lblEquipo.Location = new Point(292, 58);
            lblEquipo.Name = "lblEquipo";
            lblEquipo.Size = new Size(59, 20);
            lblEquipo.TabIndex = 3;
            lblEquipo.Text = "Equipo:";
            // 
            // cmbEquipo
            // 
            cmbEquipo.FormattingEnabled = true;
            cmbEquipo.Location = new Point(292, 81);
            cmbEquipo.Name = "cmbEquipo";
            cmbEquipo.Size = new Size(239, 28);
            cmbEquipo.TabIndex = 4;
            // 
            // lblFallaReportada
            // 
            lblFallaReportada.AutoSize = true;
            lblFallaReportada.Location = new Point(566, 58);
            lblFallaReportada.Name = "lblFallaReportada";
            lblFallaReportada.Size = new Size(116, 20);
            lblFallaReportada.TabIndex = 5;
            lblFallaReportada.Text = "Falla Reportada:";
            // 
            // txtFalla
            // 
            txtFalla.Location = new Point(566, 81);
            txtFalla.Name = "txtFalla";
            txtFalla.Size = new Size(239, 27);
            txtFalla.TabIndex = 6;
            // 
            // lblDiagnostico
            // 
            lblDiagnostico.AutoSize = true;
            lblDiagnostico.Location = new Point(12, 126);
            lblDiagnostico.Name = "lblDiagnostico";
            lblDiagnostico.Size = new Size(64, 20);
            lblDiagnostico.TabIndex = 7;
            lblDiagnostico.Text = "Servicio:";
            // 
            // cmbServicio
            // 
            cmbServicio.FormattingEnabled = true;
            cmbServicio.Location = new Point(12, 149);
            cmbServicio.Name = "cmbServicio";
            cmbServicio.Size = new Size(239, 28);
            cmbServicio.TabIndex = 8;
            // 
            // lblPlan
            // 
            lblPlan.AutoSize = true;
            lblPlan.Location = new Point(292, 126);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(40, 20);
            lblPlan.TabIndex = 9;
            lblPlan.Text = "Plan:";
            // 
            // cmbPlan
            // 
            cmbPlan.FormattingEnabled = true;
            cmbPlan.Location = new Point(292, 149);
            cmbPlan.Name = "cmbPlan";
            cmbPlan.Size = new Size(239, 28);
            cmbPlan.TabIndex = 10;
            // 
            // lblNotificaciones
            // 
            lblNotificaciones.AutoSize = true;
            lblNotificaciones.Location = new Point(566, 126);
            lblNotificaciones.Name = "lblNotificaciones";
            lblNotificaciones.Size = new Size(115, 20);
            lblNotificaciones.TabIndex = 11;
            lblNotificaciones.Text = "Aviso Simulado:";
            // 
            // cmbNotificacion
            // 
            cmbNotificacion.FormattingEnabled = true;
            cmbNotificacion.Location = new Point(566, 149);
            cmbNotificacion.Name = "cmbNotificacion";
            cmbNotificacion.Size = new Size(239, 28);
            cmbNotificacion.TabIndex = 12;
            // 
            // chkRepuesto
            // 
            chkRepuesto.AutoSize = true;
            chkRepuesto.Location = new Point(12, 192);
            chkRepuesto.Name = "chkRepuesto";
            chkRepuesto.Size = new Size(217, 24);
            chkRepuesto.TabIndex = 13;
            chkRepuesto.Text = "Agregar Repuesto Fijo ($15)";
            chkRepuesto.UseVisualStyleBackColor = true;
            // 
            // btnRegistrar
            // 
            btnRegistrar.BackColor = Color.FromArgb(128, 255, 128);
            btnRegistrar.Location = new Point(12, 232);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(189, 29);
            btnRegistrar.TabIndex = 14;
            btnRegistrar.Text = "Registrar Orden";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // btnFinalizar
            // 
            btnFinalizar.BackColor = Color.FromArgb(255, 192, 128);
            btnFinalizar.Location = new Point(207, 232);
            btnFinalizar.Name = "btnFinalizar";
            btnFinalizar.Size = new Size(189, 29);
            btnFinalizar.TabIndex = 15;
            btnFinalizar.Text = "Finalizar Orden";
            btnFinalizar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(255, 128, 128);
            btnCancelar.Location = new Point(402, 232);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(189, 29);
            btnCancelar.TabIndex = 16;
            btnCancelar.Text = "Cancelar Seleccionada";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.Silver;
            btnLimpiar.Location = new Point(597, 232);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(94, 29);
            btnLimpiar.TabIndex = 17;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // dataGridViewOrdenes
            // 
            dataGridViewOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewOrdenes.Location = new Point(12, 271);
            dataGridViewOrdenes.Name = "dataGridViewOrdenes";
            dataGridViewOrdenes.RowHeadersWidth = 51;
            dataGridViewOrdenes.Size = new Size(793, 249);
            dataGridViewOrdenes.TabIndex = 18;
            // 
            // lblEventos
            // 
            lblEventos.AutoSize = true;
            lblEventos.Location = new Point(11, 538);
            lblEventos.Name = "lblEventos";
            lblEventos.Size = new Size(240, 20);
            lblEventos.TabIndex = 19;
            lblEventos.Text = "Eventos y notificaciones simuladas:";
            // 
            // txtEventos
            // 
            txtEventos.BackColor = SystemColors.GradientInactiveCaption;
            txtEventos.Location = new Point(11, 561);
            txtEventos.Multiline = true;
            txtEventos.Name = "txtEventos";
            txtEventos.ReadOnly = true;
            txtEventos.ScrollBars = ScrollBars.Vertical;
            txtEventos.Size = new Size(794, 76);
            txtEventos.TabIndex = 20;
            // 
            // FrmReparaciones
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(827, 664);
            Controls.Add(txtEventos);
            Controls.Add(lblEventos);
            Controls.Add(dataGridViewOrdenes);
            Controls.Add(btnLimpiar);
            Controls.Add(btnCancelar);
            Controls.Add(btnFinalizar);
            Controls.Add(btnRegistrar);
            Controls.Add(chkRepuesto);
            Controls.Add(cmbNotificacion);
            Controls.Add(lblNotificaciones);
            Controls.Add(cmbPlan);
            Controls.Add(lblPlan);
            Controls.Add(cmbServicio);
            Controls.Add(lblDiagnostico);
            Controls.Add(txtFalla);
            Controls.Add(lblFallaReportada);
            Controls.Add(cmbEquipo);
            Controls.Add(lblEquipo);
            Controls.Add(txtCliente);
            Controls.Add(lblCliente);
            Controls.Add(lblTitulo);
            Name = "FrmReparaciones";
            Text = "Reparaciones Laboratorio";
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrdenes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblCliente;
        private TextBox txtCliente;
        private Label lblEquipo;
        private ComboBox cmbEquipo;
        private Label lblFallaReportada;
        private TextBox txtFalla;
        private Label lblDiagnostico;
        private ComboBox cmbServicio;
        private Label lblPlan;
        private ComboBox cmbPlan;
        private Label lblNotificaciones;
        private ComboBox cmbNotificacion;
        private CheckBox chkRepuesto;
        private Button btnRegistrar;
        private Button btnFinalizar;
        private Button btnCancelar;
        private Button btnLimpiar;
        private DataGridView dataGridViewOrdenes;
        private Label lblEventos;
        private TextBox txtEventos;
    }
}
