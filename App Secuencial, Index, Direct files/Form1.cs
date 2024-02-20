using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace App_Secuencial__Index__Direct_files
{
    public partial class Form1 : Form
    {
        public string dataSecuencialFilePath;

        public string indexSecuencialIndexFilePath;
        public string dataSecuencialIndexFilePath;

        public string dataDirectedFilePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateRegister_Click(object sender, EventArgs e)
        {
            using (var FolderPath = new FolderBrowserDialog())
            {
                if (FolderPath.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                GenerateFile(FolderPath.SelectedPath);
            }
        }

        public void GenerateFile(string path)
        {
            dataSecuencialFilePath = Path.Combine(path, "datosSecuenciales.dat");

            if (File.Exists(path))
            {
                return;
            }

            using (var FileStream = File.Create(dataSecuencialFilePath)) { }
        }

        private void AddRegistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dataSecuencialFilePath))
            {
                return;
            }

            string Id = IDtext.Text;
            string Name = Nombretext.Text;
            string Note = Notatext.Text;

            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Note))
            {
                AddRegitre(Id, Name, Note);

                IDtext.Clear();
                Nombretext.Clear();
                Notatext.Clear();

                UpdateList();
            }

            UpdateList();
        }

        private void AddRegitre(string Id, string Name, string Note)
        {
            using (var writer = new StreamWriter(dataSecuencialFilePath, true))
            {
                writer.WriteLine($"{Id},{Name},{Note}");
            }
        }

        private void UpdateList()
        {
            listRegistrerData.Items.Clear();

            if (File.Exists(dataSecuencialFilePath))
            {
                var Regitre = new List<string>(File.ReadAllLines(dataSecuencialFilePath));

                listRegistrerData.Items.AddRange(Regitre.ToArray());
            }
        }

        private void OpenRegistrer_Click(object sender, EventArgs e)
        {
            var OpenFile = new OpenFileDialog();

            if (OpenFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dataSecuencialFilePath = OpenFile.FileName;

            UpdateList();
        }

        private void secuencialMenuItem_Click(object sender, EventArgs e)
        {
            PanelIndexados.Visible = false;
        }

        private void CreateRegisterIndexed_Click(object sender, EventArgs e)
        {
            using (var FolderPath = new FolderBrowserDialog())
            {
                if (FolderPath.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                GenerateFileIndexed(FolderPath.SelectedPath);
            }
        }

        private void GenerateFileIndexed(string path)
        {
            indexSecuencialIndexFilePath = Path.Combine(path, "indice.dat");
            dataSecuencialIndexFilePath = Path.Combine(path, "datosIndexados.dat");

            if (File.Exists(indexSecuencialIndexFilePath))
            {
                return;
            }

            using (var fs = File.Create(indexSecuencialIndexFilePath)) { }

            if (File.Exists(dataSecuencialIndexFilePath))
            {
                return;
            }

            using (var fs = File.Create(dataSecuencialIndexFilePath)) { }
        }

        private void OpenRegisterIndexed_Click(object sender, EventArgs e)
        {
            var OpenFile = new OpenFileDialog();

            if (OpenFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            indexSecuencialIndexFilePath = OpenFile.FileName;

            if (OpenFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dataSecuencialIndexFilePath = OpenFile.FileName;

            UpdateListIndexed();
        }

        private void AddRegisterIndexed_Click(object sender, EventArgs e)
        {
            string clave = KeyText.Text;
            string dato = DataText.Text;

            if (!string.IsNullOrEmpty(clave) && !string.IsNullOrEmpty(dato))
            {
                AddregisterIndexed(clave, dato);

                KeyText.Clear();
                DataText.Clear();

                UpdateListIndexed();
            }
        }

        private void AddregisterIndexed(string clave, string dato)
        {
            using (var dataWriter = new StreamWriter(dataSecuencialIndexFilePath, true))
            {
                dataWriter.WriteLine($"{clave},{dato}");
            }

            using (var indexWriter = new StreamWriter(indexSecuencialIndexFilePath, true))
            {
                long offset = ObtenerOffset(dataSecuencialIndexFilePath);
                indexWriter.WriteLine($"{clave}:{dato}:{offset}");
            }
        }

        private void UpdateListIndexed()
        {
            listRegistrerIndexedIndex.Items.Clear();
            listRegistrerIndexedData.Items.Clear();

            if (!File.Exists(indexSecuencialIndexFilePath))
            {
                return;
            }

            var indexLines = new List<string>(File.ReadAllLines(indexSecuencialIndexFilePath));

            foreach (string line in indexLines)
            {
                string[] parts = line.Split(':');

                listRegistrerIndexedIndex.Items.Add($"{parts[0]} : {parts[1]} : {parts[2]}");
            }

            if (!File.Exists(dataSecuencialIndexFilePath))
            {
                return;
            }

            var dataLines = new List<string>(File.ReadAllLines(dataSecuencialIndexFilePath));

            foreach (string line in indexLines)
            {
                string[] parts = line.Split(':');

                listRegistrerIndexedData.Items.Add($"{parts[0]} - {parts[1]}");
            }
        }

        private long ObtenerOffset(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return fs.Length;
            }
        }

        private void indexadosMenuItem_Click(object sender, EventArgs e)
        {
            PanelIndexados.Visible = true;
            PanelDirectos.Visible = false;
        }

        private void CrearRegistroDirectos_Click(object sender, EventArgs e)
        {
            using (var FolderPath = new FolderBrowserDialog())
            {
                if (FolderPath.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                GenerateFileDirected(FolderPath.SelectedPath);
            }
        }

        public void GenerateFileDirected(string path)
        {
            dataDirectedFilePath = Path.Combine(path, "datosAccesoDirecto.dat");

            if (File.Exists(path))
            {
                return;
            }

            using (var FileStream = File.Create(dataDirectedFilePath)) { }
        }

        private void AbrirRegistrosDirectos_Click(object sender, EventArgs e)
        {
            var OpenFile = new OpenFileDialog();

            if (OpenFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dataDirectedFilePath = OpenFile.FileName;

            UpdateListDirected();
        }

        private void AgregarRegistrosDirectos_Click(object sender, EventArgs e)
        {
            string id = IDTextDirected.Text.PadRight(10);
            string nombre = NombreTextDirected.Text.PadRight(50);
            string nota = NotaTextDirected.Text.PadRight(40);

            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(nota))
            {
                return;
            }

            AddRegistredDirected(id, nombre, nota);

            IDTextDirected.Clear();
            NombreTextDirected.Clear();
            NotaTextDirected.Clear();

            UpdateListDirected();
        }

        private void AddRegistredDirected(string id, string nombre, string nota)
        {
            using (FileStream fileStream = new FileStream(dataDirectedFilePath, FileMode.Append, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(Encoding.UTF8.GetBytes(id));
                writer.Write(Encoding.UTF8.GetBytes(nombre));
                writer.Write(Encoding.UTF8.GetBytes(nota));
            }
        }

        private void UpdateListDirected()
        {
            listRegistrerDataDirected.Items.Clear();

            using (FileStream fileStream = new FileStream(dataDirectedFilePath, FileMode.Open, FileAccess.Read))
            {
                long fileSize = fileStream.Length;

                while (fileStream.Position < fileSize)
                {
                    using (BinaryReader reader = new BinaryReader(fileStream, Encoding.UTF8, true))
                    {
                        string id = Encoding.UTF8.GetString(reader.ReadBytes(10)).Trim();
                        string nombre = Encoding.UTF8.GetString(reader.ReadBytes(50)).Trim();
                        string nota = Encoding.UTF8.GetString(reader.ReadBytes(40)).Trim();

                        listRegistrerDataDirected.Items.Add($"ID: {id}, Nombre: {nombre}, Nota: {nota}");
                    }
                }
            }
        }

        private void directosMenuItem_Click(object sender, EventArgs e)
        {
            PanelIndexados.Visible = true;
            PanelDirectos.Visible = true;
        }
    }
}
