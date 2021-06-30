Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        removeFromCartLink: "",
    },
    
    init: function(properties) {
        $.extend(Cart._properties, properties);
        
        Cart.initEvents();
    },
    
    initEvents: function (){
        $(".add-to-cart").click(Cart.addToCart);
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
        
        const value = total.toLocaleString("ru-Ru", {style:"currency", currency: "RUB"})
        $("#total-order-sum").html(value)
    }
}