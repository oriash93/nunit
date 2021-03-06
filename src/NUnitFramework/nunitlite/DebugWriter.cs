// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.Diagnostics;
using System.IO;

namespace NUnitLite
{
    /// <summary>
    /// DebugWriter is a TextWriter that sends its
    /// output to Debug. We don't use Trace because
    /// writing to it is not supported in CF.
    /// </summary>
    [Obsolete("No longer used")]
    public class DebugWriter : TextWriter
    {
        private static TextWriter writer;

        /// <summary>
        /// Singleton instance of a DebugWriter.
        /// </summary>
        /// <value>The DebugWriter singleton.</value>
        public static TextWriter Out
        {
            get
            {
                if (writer == null)
                    writer = new DebugWriter();

                return writer;
            }
        }

        /// <summary>
        /// Writes a character to the text stream.
        /// </summary>
        /// <param name="value">The character to write to the text stream.</param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:System.IO.TextWriter"/> is closed.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        public override void Write(char value)
        {
            Debug.Write(value);
        }

        /// <summary>
        /// Writes a string to the text stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:System.IO.TextWriter"/> is closed.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        public override void Write(string value)
        {
            Debug.Write(value);
        }

        /// <summary>
        /// Writes a string followed by a line terminator to the text stream.
        /// </summary>
        /// <param name="value">The string to write. If <paramref name="value"/> is null, only the line termination characters are written.</param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:System.IO.TextWriter"/> is closed.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"/> in which the output is written.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The Encoding in which the output is written.
        /// </returns>
        public override System.Text.Encoding Encoding { get; } = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    }
}
