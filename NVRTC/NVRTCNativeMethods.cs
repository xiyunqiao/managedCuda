﻿//	Copyright (c) 2014, Michael Kunz. All rights reserved.
//	http://kunzmi.github.io/managedCuda
//
//	This file is part of ManagedCuda.
//
//	ManagedCuda is free software: you can redistribute it and/or modify
//	it under the terms of the GNU Lesser General Public License as 
//	published by the Free Software Foundation, either version 2.1 of the 
//	License, or (at your option) any later version.
//
//	ManagedCuda is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//	GNU Lesser General Public License for more details.
//
//	You should have received a copy of the GNU Lesser General Public
//	License along with this library; if not, write to the Free Software
//	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
//	MA 02110-1301  USA, http://www.gnu.org/licenses/.


using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using ManagedCuda;
using ManagedCuda.BasicTypes;

namespace ManagedCuda.NVRTC
{
	/// <summary/>
	public static class NVRTCNativeMethods
	{
#if _x64
		internal const string NVRTC_API_DLL_NAME = "nvrtc64_75.dll";
#else
		internal const string NVRTC_API_DLL_NAME = "nvrtc32_75.dll";
#endif
		
		[DllImport(NVRTC_API_DLL_NAME, EntryPoint="nvrtcGetErrorString")]
        internal static extern IntPtr nvrtcGetErrorStringInternal(nvrtcResult result);

		/// <summary>
		/// helper function that stringifies the given #nvrtcResult code, e.g., NVRTC_SUCCESS to
		/// "NVRTC_SUCCESS". For unrecognized enumeration values, it returns "NVRTC_ERROR unknown"
		/// </summary>
		/// <param name="result">CUDA Runtime Compiler API result code.</param>
		/// <returns>Message string for the given nvrtcResult code.</returns>
		public static string nvrtcGetErrorString(nvrtcResult result)
		{
			IntPtr ptr = nvrtcGetErrorStringInternal(result);
			return Marshal.PtrToStringAnsi(ptr);
		}


		/// <summary>
		/// sets the output parameters \p major and \p minor
		/// with the CUDA Runtime Compiler version number.
		/// </summary>
		/// <param name="major">CUDA Runtime Compiler major version number.</param>
		/// <param name="minor">CUDA Runtime Compiler minor version number.</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcVersion(ref int major, ref int minor);


		/// <summary>
		/// creates an instance of ::nvrtcProgram with the
		/// given input parameters, and sets the output parameter \p prog with it.
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <param name="src">CUDA program source.</param>
		/// <param name="name">CUDA program name.<para/>
		/// name can be NULL; "default_program" is used when name is NULL.</param>
		/// <param name="numHeaders">Number of headers used.<para/>
		/// numHeaders must be greater than or equal to 0.</param>
		/// <param name="headers">Sources of the headers.<para/>
		/// headers can be NULL when numHeaders is 0.</param>
		/// <param name="includeNames">Name of each header by which they can be included in the CUDA program source.<para/>
		/// includeNames can be NULL when numHeaders is 0.</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcCreateProgram(ref nvrtcProgram prog,
                               [MarshalAs(UnmanagedType.AnsiBStr)] string src,
                               [MarshalAs(UnmanagedType.AnsiBStr)] string name,
                               int numHeaders,
                               IntPtr[] headers,
                               IntPtr[] includeNames);




		/// <summary>
		/// destroys the given program.
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcDestroyProgram(ref nvrtcProgram prog);

		/// <summary>
		/// compiles the given program.
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <param name="numOptions">Number of compiler options passed.</param>
		/// <param name="options">Compiler options in the form of C string array.<para/>
		/// options can be NULL when numOptions is 0.</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcCompileProgram(nvrtcProgram prog, int numOptions, IntPtr[] options);

		/// <summary>
		/// sets \p ptxSizeRet with the size of the PTX generated by the previous compilation of prog (including the trailing NULL).
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <param name="ptxSizeRet">Size of the generated PTX (including the trailing NULL).</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcGetPTXSize(nvrtcProgram prog, ref SizeT ptxSizeRet);

		/// <summary>
		/// stores the PTX generated by the previous compilation
		/// of prog in the memory pointed by ptx.
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <param name="ptx">Compiled result.</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcGetPTX(nvrtcProgram prog, byte[] ptx);

		/// <summary>
		/// sets logSizeRet with the size of the log generated by the previous compilation of prog (including the trailing NULL).
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <param name="logSizeRet">Size of the compilation log (including the trailing NULL).</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcGetProgramLogSize(nvrtcProgram prog, ref SizeT logSizeRet);

		/// <summary>
		/// stores the log generated by the previous compilation of prog in the memory pointed by log.
		/// </summary>
		/// <param name="prog">CUDA Runtime Compiler program.</param>
		/// <param name="log">Compilation log.</param>
		/// <returns></returns>
		[DllImport(NVRTC_API_DLL_NAME)]
		public static extern nvrtcResult nvrtcGetProgramLog(nvrtcProgram prog, byte[] log);
	}
}
