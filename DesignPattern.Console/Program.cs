using System;
using Newtonsoft.Json.Linq;

namespace DesignPattern.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[][] files = new byte[int.MaxValue][];
            IScannerAdapter scannerAdapter = new McAfeeScannerAdapter(files[0]);
            string result = scannerAdapter.Scan();

            scannerAdapter = new SemantecScannerAdapter(files);
            result = scannerAdapter.Scan();
        }
    }

    public class McAfeeScannerAdapter:IScannerAdapter
    {
        McAfeeScanner scanner;
        readonly bool isFileScan;
        readonly bool isFolderScan;

        public McAfeeScannerAdapter(byte[] file)
        {
            this.scanner = new McAfeeScanner(file);
            this.isFileScan = true;
            this.isFolderScan = false;
        }

        public McAfeeScannerAdapter(byte[][] files)
        {
            this.scanner = new McAfeeScanner(files);
            this.isFileScan = false;
            this.isFolderScan = true;
        }

        public string Scan()
        {
            JObject result = null;
            if (this.isFileScan)
                result = this.scanner.ScanFiles();
            if (this.isFolderScan)
                result = this.scanner.ScanFolders();
            return result.ToString();
        }
    }

    public class SemantecScannerAdapter : IScannerAdapter
    {
        SemantecScanner scanner;
        readonly bool isFileScan;
        readonly bool isFolderScan;

        public SemantecScannerAdapter(byte[] file)
        {
            this.scanner = new SemantecScanner(file);
            this.isFileScan = true;
            this.isFolderScan = false;
        }

        public SemantecScannerAdapter(byte[][] files)
        {
            this.scanner = new SemantecScanner(files);
            this.isFileScan = false;
            this.isFolderScan = true;
        }

        public string Scan()
        {
            JObject result = null;
            if (this.isFileScan)
                result = this.scanner.ScanFiles();
            if (this.isFolderScan)
                result = this.scanner.ScanFolders();
            return result.ToString();
        }
    }

    public interface IScannerAdapter
    {
        string Scan();
    }

    public sealed class McAfeeScanner
    {
        byte[][] files;

        public McAfeeScanner(byte[] file)
        {
            files = new byte[file.Length][];
            this.files[0] = file;
        }

        public McAfeeScanner(byte[][] files)
        {
            this.files = new byte[files.Length][];
            this.files = files;
        }

        public JObject ScanFolders()
        {
            return new JObject();
        }

        public JObject ScanFiles()
        {
            return new JObject();
        }
    }

    public sealed class SemantecScanner
    {
        byte[][] files;

        public SemantecScanner(byte[] file)
        {
            files = new byte[file.Length][];
            this.files[0] = file;
        }

        public SemantecScanner(byte[][] files)
        {
            this.files = new byte[files.Length][];
            this.files = files;
        }

        public JObject ScanFolders()
        {
            return new JObject();
        }

        public JObject ScanFiles()
        {
            return new JObject();
        }
    }
}
