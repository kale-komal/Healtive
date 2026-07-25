using Healtive.Application.DTOs.Hospital;
using Healtive.Application.DTOs.Subscription;
using Healtive.Application.DTOs.SubscriptionPlan;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Services.HospitalSubscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Healtive.API.Controllers;

[Authorize(Roles = "SuperAdmin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IHospitalService _hospitalService;
    private readonly ISubscriptionPlanService _subscriptionPlanService;
    private readonly IHospitalSubscriptionService _hospitalSubscriptionService;
    private readonly IDashboardService _dashboardService;

    public AdminController(
        IHospitalService hospitalService,
        ISubscriptionPlanService subscriptionPlanService,
        IHospitalSubscriptionService hospitalSubscriptionService,
        IDashboardService dashboardService)
    {
        _hospitalService = hospitalService;
        _subscriptionPlanService = subscriptionPlanService;
        _hospitalSubscriptionService = hospitalSubscriptionService;
        _dashboardService = dashboardService;
    }

    [HttpPost("hospitals")]
    public async Task<IActionResult> CreateHospital(
        CreateHospitalRequest request)
    {
        var result = await _hospitalService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("hospitals/{id:guid}")]
    public async Task<IActionResult> GetHospital(Guid id)
    {
        var result = await _hospitalService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("hospitals")]
    public async Task<IActionResult> GetHospitals()
    {
        var result = await _hospitalService.GetAllAsync();

        return Ok(result);
    }

    [HttpPut("hospitals/{id:guid}")]
    public async Task<IActionResult> UpdateHospital(
    Guid id,
    UpdateHospitalRequest request)
    {
        var result = await _hospitalService.UpdateAsync(id, request);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("hospitals/{id:guid}")]
    public async Task<IActionResult> DeleteHospital(Guid id)
    {
        var result = await _hospitalService.DeleteAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPatch("hospitals/{id:guid}/activate")]
    public async Task<IActionResult> ActivateHospital(Guid id)
    {
        var result = await _hospitalService.ActivateAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("hospitals/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateHospital(Guid id)
    {
        var result = await _hospitalService.DeactivateAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    //====================================================
    // Subscription Plans
    //====================================================

    [HttpGet("subscription-plans")]
    public async Task<IActionResult> GetSubscriptionPlans()
    {
        var result = await _subscriptionPlanService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("subscription-plans/{id:guid}")]
    public async Task<IActionResult> GetSubscriptionPlan(Guid id)
    {
        var result = await _subscriptionPlanService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("subscription-plans")]
    public async Task<IActionResult> CreateSubscriptionPlan(
        CreateSubscriptionPlanRequest request)
    {
        var result = await _subscriptionPlanService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("subscription-plans/{id:guid}")]
    public async Task<IActionResult> UpdateSubscriptionPlan(
        Guid id,
        UpdateSubscriptionPlanRequest request)
    {
        var result = await _subscriptionPlanService.UpdateAsync(id, request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("subscription-plans/{id:guid}")]
    public async Task<IActionResult> DeleteSubscriptionPlan(Guid id)
    {
        var result = await _subscriptionPlanService.DeleteAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    // =============================
    // Hospital Subscription APIs
    // =============================
    

    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        var result = await _hospitalSubscriptionService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("subscriptions/{id:guid}")]
    public async Task<IActionResult> GetSubscription(Guid id)
    {
        var result = await _hospitalSubscriptionService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("subscriptions")]
    public async Task<IActionResult> CreateSubscription(
        CreateHospitalSubscriptionRequest request)
    {
        var result = await _hospitalSubscriptionService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("subscriptions/{id:guid}")]
    public async Task<IActionResult> UpdateSubscription(
        Guid id,
        UpdateHospitalSubscriptionRequest request)
    {
        var result = await _hospitalSubscriptionService.UpdateAsync(id, request);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("subscriptions/{id:guid}")]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        var result = await _hospitalSubscriptionService.DeleteAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPatch("subscriptions/{id:guid}/renew")]
    public async Task<IActionResult> RenewSubscription(Guid id)
    {
        var result = await _hospitalSubscriptionService.RenewAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPatch("subscriptions/{id:guid}/cancel")]
    public async Task<IActionResult> CancelSubscription(Guid id)
    {
        var result = await _hospitalSubscriptionService.CancelAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _dashboardService.GetDashboardAsync();

        return Ok(result);
    }
}