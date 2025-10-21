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

using System;
using MicroElements.AutoFixture.NodaTime;
using NodaTime;
using Yuma.AutoFixture.Xunit2;

namespace Yuma;

public class ClockProviderFixture
{
	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupGetCurrentInstant(Instant instant)
	{
		var systemClockProvider = ClockProvider.Instance;

		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.GetCurrentInstant())
			.Returns(instant);

		ClockProvider.Instance.GetCurrentInstant()
			.Should()
			.Be(instant);

		ClockProvider.Instance.GetCurrentInstant()
			.Should()
			.NotBe(systemClockProvider.GetCurrentInstant());
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupNowExplicitly(Instant now)
	{
		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.Now)
			.Returns(now);

		ClockProvider.Instance.Now.Should()
			.Be(now);
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupNowImplicitly(Instant instant)
	{
		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.GetCurrentInstant())
			.Returns(instant);

		ClockProvider.Instance.Now.Should()
			.Be(instant);
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupTodayExplicitly(LocalDate today)
	{
		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.Today)
			.Returns(today);

		ClockProvider.Instance.Today.Should()
			.Be(today);
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupTodayImplicitly(Instant instant)
	{
		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.GetCurrentInstant())
			.Returns(instant);

		ClockProvider.Instance.Today.Should()
			.Be(
				instant.InZone(ClockProvider.SystemDefaultDateTimeZone)
					.Date);
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupUtcNowExplicitly(ZonedDateTime now)
	{
		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.UtcNow)
			.Returns(now);

		ClockProvider.Instance.UtcNow.Should()
			.Be(now);
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void CanSetupUtcNowImplicitly(Instant instant)
	{
		using var clockProvider = new ClockProviderMockInjectionScope();
		clockProvider.Mock.Setup(static m => m.GetCurrentInstant())
			.Returns(instant);

		ClockProvider.Instance.UtcNow.Should()
			.Be(instant.InUtc());
	}

	[Fact]
	public void GetCurrentInstantReturnsSystemClockProviderInstant()
	{
		ClockProvider.Instance.GetCurrentInstant()
			.ToDateTimeOffset()
			.Should()
			.BeCloseTo(
				SystemClock.Instance.GetCurrentInstant()
					.ToDateTimeOffset(),
				TimeSpan.FromSeconds(seconds: 3));
	}

	[Fact]
	public void MockInjectionScopeShadowsAndRestoresSystemClockProvider()
	{
		var systemClockProvider = ClockProvider.Instance;
		using (new ClockProviderMockInjectionScope())
		{
			ClockProvider.Instance.Should()
				.NotBe(systemClockProvider);
		}
		ClockProvider.Instance.Should()
			.Be(systemClockProvider);
	}
}
