Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        removeFromCartLink: "",
        decrementLink: "",
        removeAllLink: ""
    },
    
    init: function(properties) {
        $.extend(Cart._properties, properties);
        
        Cart.initEvents();
    },
    
    initEvents: function (){
        $(".add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_up").on("click", Cart.incrementItem);
        $(".cart_quantity_down").on("click", Cart.decrementItem);
        $(".cart_quantity_delete").on("click", Cart.removeFromCart);
    },
    
    addToCart: function(event) {
        event.preventDefault();

        const button = $(this);
        const id = button.data("id");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function(){
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function(){
                console.log("addToCart failed");
            })
    },
    incrementItem: function(e){
        e.preventDefault();

        const button = $(this);
        const id = button.data("id");
        
        const container = button.closest("tr");
        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function(){
                const count = parseInt($(".cart_quantity_input", container).val())
                $(".cart_quantity_input", container).val(count + 1)
                Cart.refreshPrice(container)
                Cart.refreshCartView()
            })
            .fail(function(){
                console.log("incrementItem failed");
            })
    },
    
    decrementItem: function (e){
        e.preventDefault();

        const button = $(this);
        const id = button.data("id");

        const container = button.closest("tr");
        $.get(Cart._properties.decrementLink + "/" + id)
            .done(function(){
                const count = parseInt($(".cart_quantity_input", container).val())
                if (count > 1){
                    $(".cart_quantity_input", container).val(count - 1)
                    Cart.refreshPrice(container);
                    Cart.refreshCartView();
                } else {
                    container.remove()
                    Cart.refreshTotalPrice()
                }
            })
            .fail(function(){
                console.log("decrementItem failed");
            })
    },

    refreshPrice: function(container){
        const quantity = parseInt($(".cart_quantity_input", container).val())
        const price = parseFloat($(".cart_price", container).data("price"))
        const totalPrice = quantity * price;
        
        const value = totalPrice.toLocaleString("ru-RU",{style:"currency", currency: "RUB"})
        $(".cart_total_price", container).data("price", totalPrice)
        $(".cart_total_price", container).html(value)
        
        Cart.refreshTotalPrice()
    },

    showToolTip: function(button) {
        button.tooltip({title: "Added to cart"}).tooltip("show");
        setTimeout(function(){
            button.tooltip("destroy")
        }, 500);
    },
    
    refreshCartView: function() {
        const container = $("#cartContainer");
        $.get(Cart._properties.getCartViewLink)
            .done(function(result){
                container.html(result)
            })
            .fail(function() {
                console.log("refreshCartView failed")
            })
    },
    removeFromCart: function(e){
        e.preventDefault();
        
        const button = $(this);
        const id = button.data("id");
        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done(function(){
                button.closest("tr").remove()
                Cart.refreshTotalPrice()
            })
            .fail(function() {console.log("removeFromCart failed")})
     },

    refreshTotalPrice: function(){
        let total = 0
        
        $(".cart_total_price").each(
            function(){
                const price = parseFloat($(this).data("price"))
                total += price
            }
        )
        
        const value = total.toLocaleString("ru-RU", {style:"currency", currency: "RUB"})
        $("#total-order-sum").html(value)
    }
}