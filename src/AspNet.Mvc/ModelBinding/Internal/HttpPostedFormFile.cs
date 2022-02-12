// Portions Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/LICENSE.txt for license information.
// https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Http/Http/src/Internal/FormFile.cs

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Unravel.AspNet.Mvc.ModelBinding.Internal
{
    /// <summary>
    /// <see cref="IFormFile"/> wrapper for an <see cref="HttpPostedFileBase"/>.
    /// </summary>
    public class HttpPostedFormFile : IFormFile
    {
        // Stream.CopyTo method uses 80KB as the default buffer size.
        private const int DefaultBufferSize = 80 * 1024;

        /// <summary>
        /// Initializes a new <see cref="HttpPostedFormFile"/>.
        /// </summary>
        /// <param name="name">The form field name.</param>
        /// <param name="file">The uploaded file.</param>
        public HttpPostedFormFile(string name, HttpPostedFileBase file)
        {
            Name = name;
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        /// <summary>
        /// The wrapped <see cref="HttpPostedFileBase"/>.
        /// </summary>
        public HttpPostedFileBase File { get; }

        /// <inheritdoc/>
        public string ContentType => File.ContentType;

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public string ContentDisposition => throw new NotSupportedException();

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public IHeaderDictionary Headers => throw new NotSupportedException();

        /// <inheritdoc/>
        public long Length => File.ContentLength;

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string FileName => File.FileName;

        public Stream OpenReadStream()
        {
            return new ReferenceReadStream(File.InputStream, 0, Length);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        public void CopyTo(Stream target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            using (var readStream = OpenReadStream())
            {
                readStream.CopyTo(target, DefaultBufferSize);
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            using (var readStream = OpenReadStream())
            {
                await readStream.CopyToAsync(target, DefaultBufferSize, cancellationToken);
            }
        }
    }
}
