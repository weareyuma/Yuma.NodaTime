#region Copyright & License

// Copyright © 2024 - 2025 Yuma
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Diagnostics.CodeAnalysis;
using NodaTime;

namespace Yuma;

[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Public API.")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global", Justification = "Required for mocking purposes.")]
public class ClockProvider : IClockProvider
{
	/// <summary><see cref="ClockProvider"/> singleton instance.</summary>
	/// <remarks>For unit tests purposes, see its <c>Yuma.NodaTime.Unit, Yuma.ClockProviderMockInjectionScope</c> testing buddy.</remarks>
	public static IClockProvider Instance { get; internal set; } = new ClockProvider();

	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Required for mocking purposes.")]
	internal ClockProvider() { }

	#region IClockProvider Members

	public virtual Instant GetCurrentInstant()
	{
		return SystemClock.Instance.GetCurrentInstant();
	}

	public Instant Now => GetCurrentInstant();

	// @formatter:wrap_chained_method_calls chop_if_long
	public LocalDate Today => Now.InZone(SystemDefaultDateTimeZone).Date;
	// @formatter:wrap_chained_method_calls restore

	public ZonedDateTime UtcNow => Now.InUtc();

	#endregion

	internal static readonly DateTimeZone SystemDefaultDateTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
}
