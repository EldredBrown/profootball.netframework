using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

/// <summary>
/// Credit to: https://referencesource.microsoft.com/#System.ComponentModel.DataAnnotations/DataAnnotations/CompareAttribute.cs
/// </summary>
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Provides an attribute that compares two properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CompareForInequalityAttribute : ValidationAttribute
    {
        private const string defaultErrorMessage = "{0} must be different from {1}.";

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.DataAnnotations.CompareForInequalityAttribute class.
        /// </summary>
        /// <param name="otherProperty">
        /// The property to compare with the current property.
        /// </param>
        public CompareForInequalityAttribute(string otherProperty)
            : base(defaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");

            }
            OtherProperty = otherProperty;
        }

        /// <summary>
        /// Gets the property to compare with the current property.
        /// </summary>
        /// <returns>
        /// The other property.
        /// </returns>
        public string OtherProperty { get; private set; }

        /// <summary>
        /// Gets the display name of the other property.
        /// </summary>
        /// <returns>
        /// The display name of the other property.
        /// </returns>
        public string OtherPropertyDisplayName { get; internal set; }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        /// <returns>
        /// true if the attribute requires validation context; otherwise, false.
        /// </returns>
        public override bool RequiresValidationContext
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">
        /// The name of the field that caused the validation failure.
        /// </param>
        /// <returns>
        /// The formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name,
                OtherPropertyDisplayName ?? OtherProperty);
        }

        /// <summary>
        /// Determines whether a specified object is valid.
        /// </summary>
        /// <param name="value">
        /// The object to validate.
        /// </param>
        /// <param name="validationContext">
        /// An object that contains information about the validation request.
        /// </param>
        /// <returns>
        /// true if value is valid; otherwise, false.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Property '{0}' is undefined.",
                    OtherProperty));
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (Equals(value, otherPropertyValue))
            {
                if (String.IsNullOrEmpty(OtherPropertyDisplayName))
                {
                    OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);
                }
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(containerType);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(propertyName, true);
            if (property == null)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Property '{0}' not found.",
                    containerType.FullName, propertyName));
            }
            IEnumerable<Attribute> attributes = property.Attributes.Cast<Attribute>();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.GetName();
            }
            DisplayNameAttribute displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            return propertyName;
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }
    }
}
