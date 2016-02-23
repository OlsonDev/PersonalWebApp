using System;

namespace PersonalWebApp.Models.Attributes {
	/// <summary>
	/// Similar to System.ComponentModel.DataAnnotations.Schema.TableAttribute except the Table name is defaulted for you.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class SchemaNameAttribute : Attribute {
		public string SchemaName { get; private set; }
		public SchemaNameAttribute(string schemaName) {
			SchemaName = schemaName;
		}
	}
}