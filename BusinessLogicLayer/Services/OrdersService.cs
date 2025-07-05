using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Validators;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
    private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
    private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
    private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;

    public OrdersService(IOrdersRepository ordersRepository,
                         IMapper mapper,
                         IValidator<OrderAddRequest> orderAddRequestValidator,
                         IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
                         IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
                         IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator)
    {
        _ordersRepository = ordersRepository;
        _mapper = mapper;
        _orderAddRequestValidator = orderAddRequestValidator;
        _orderItemAddRequestValidator = orderItemAddRequestValidator;
        _orderUpdateRequestValidator = orderUpdateRequestValidator;
        _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
    }



    public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
    {
        if (orderAddRequest == null)
        {
            throw new ArgumentNullException(nameof(orderAddRequest));
        }

        ValidationResult orderAddRequestValidationResult = await _orderAddRequestValidator.ValidateAsync(orderAddRequest);

        if(!orderAddRequestValidationResult.IsValid)
        {
            string errors = string.Join(",", orderAddRequestValidationResult.Errors.Select(vf => vf.ErrorMessage));
            throw new ArgumentException(errors);
        }

        foreach (OrderItemAddRequest orderItemAddRequest in orderAddRequest.OrderItems)
        {
            ValidationResult orderItemAddRequestValidationResult = await _orderItemAddRequestValidator.ValidateAsync(orderItemAddRequest);
            if (!orderItemAddRequestValidationResult.IsValid)
            {
                string errors = string.Join(",", orderItemAddRequestValidationResult.Errors.Select(vf => vf.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        // TO DO : Add logic for checking UserId exists in Users Microservice or not

        // Calculate total price

        Order? orderInput = _mapper.Map<Order>(orderAddRequest);

        foreach (OrderItem orderItem in orderInput.OrderItems)
        {
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
        }

        orderInput.TotalBill = orderInput.OrderItems.Sum(oi => oi.TotalPrice);

        Order? addedOrder = await _ordersRepository.AddOrder(orderInput);


        if (addedOrder == null)
        {
            return null;
        }

        return _mapper.Map<OrderResponse>(addedOrder);
    }

    public Task<bool> DeleteOrder(Guid orderID)
    {
        throw new NotImplementedException();
    }

    public Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderResponse?>> GetOrders()
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderResponse?> UpdateOrder(DTO.OrderUpdateRequest orderUpdateRequest)
    {
        if (orderUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(orderUpdateRequest));
        }

        ValidationResult orderUpdateRequestValidationResult = await _orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);

        if (!orderUpdateRequestValidationResult.IsValid)
        {
            string errors = string.Join(",", orderUpdateRequestValidationResult.Errors.Select(vf => vf.ErrorMessage));
            throw new ArgumentException(errors);
        }

        foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
        {
            ValidationResult orderItemUpdateRequestValidationResult = await _orderItemUpdateRequestValidator.ValidateAsync(orderItemUpdateRequest);
            if (!orderItemUpdateRequestValidationResult.IsValid)
            {
                string errors = string.Join(",", orderItemUpdateRequestValidationResult.Errors.Select(vf => vf.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        // TO DO : Add logic for checking UserId exists in Users Microservice or not

        // Calculate total price

        Order? orderInput = _mapper.Map<Order>(orderUpdateRequest);

        foreach (OrderItem orderItem in orderInput.OrderItems)
        {
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
        }

        orderInput.TotalBill = orderInput.OrderItems.Sum(oi => oi.TotalPrice);

        Order? updatedOrder = await _ordersRepository.UpdateOrder(orderInput);


        if (updatedOrder == null)
        {
            return null;
        }

        return _mapper.Map<OrderResponse>(updatedOrder);

    }
}
