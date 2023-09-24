using System;
using System.IO;

namespace PowerUps_change
{
	// Token: 0x0200000A RID: 10
	public class Config<T>
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002C74 File Offset: 0x00000E74
		public Config(string filePath, T defaultValue)
		{
			this.filePath = filePath;
			this.value = defaultValue;
			string directoryName = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			this.watcher = new FileSystemWatcher(directoryName);
			this.watcher.Filter = Path.GetFileName(filePath);
			this.watcher.NotifyFilter = NotifyFilters.LastWrite;
			this.watcher.Changed += this.FileChanged;
			this.watcher.EnableRaisingEvents = true;
			if (File.Exists(filePath))
			{
				string input = File.ReadAllText(filePath);
				T t;
				if (this.TryParseValue(input, out t))
				{
					this.value = t;
					return;
				}
			}
			else
			{
				File.WriteAllText(filePath, this.value.ToString());
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000020DF File Offset: 0x000002DF
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000020E7 File Offset: 0x000002E7
		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				File.WriteAllText(this.filePath, value.ToString());
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002D34 File Offset: 0x00000F34
		private void FileChanged(object sender, FileSystemEventArgs e)
		{
			string input = File.ReadAllText(this.filePath);
			T t;
			if (this.TryParseValue(input, out t))
			{
				this.value = t;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D60 File Offset: 0x00000F60
		private bool TryParseValue(string input, out T parsedValue)
		{
			bool result;
			try
			{
				parsedValue = (T)((object)Convert.ChangeType(input, typeof(T)));
				result = true;
			}
			catch
			{
				parsedValue = default(T);
				result = false;
			}
			return result;
		}

		// Token: 0x0400000D RID: 13
		private string filePath;

		// Token: 0x0400000E RID: 14
		private T value;

		// Token: 0x0400000F RID: 15
		private FileSystemWatcher watcher;
	}
}
