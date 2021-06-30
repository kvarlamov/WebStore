Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: ""
    },
    
    init: function(properties) {
        $.extend(Cart._properties, properties);
        
        Cart.initAddToCart();
    },
    
    initAddToCart: function (){
        $(".add-to-cart").click(Cart.addToCart);
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
    }
}