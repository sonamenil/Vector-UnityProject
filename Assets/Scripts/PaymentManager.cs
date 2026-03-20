using System.Collections.Generic;
using UnityEngine.Purchasing;

public class PaymentManager : AbstractManager<PaymentManager>
{
	public class PaymentProductData
	{
		//public readonly int Value;

		//public readonly string ID;

		//public readonly ProductData ProductData;

		//public PaymentProductData(string id, ProductData productData, int value)
		//{
		//}
	}

	private static IStoreController _mStoreController;

	private static IExtensionProvider _mStoreExtensionProvider;

	private static PaymentManager _current;

	private List<PaymentProductData> _products;

	private bool _initializationWas;

	public bool InitializationWas => false;

	//private IGooglePlayStoreExtensions GetGooglePlayStoreExtensions()
	//{
	//	return null;
	//}

	public List<PaymentProductData> GetProducts()
	{
		return null;
	}

	public PaymentProductData GetProductByID(string id)
	{
		return null;
	}

	public PaymentProductData GetPaymentProductDataByID(string id)
	{
		return null;
	}

	public ProductCollection GetDefinitions()
	{
		return null;
	}

	public bool IsDefinitions()
	{
		return false;
	}

	public Product GetDefinitionByID(string id)
	{
		return null;
	}

	protected override void InitInternal()
	{
	}

	public void InitializePurchasing(KeyValuePair<string, string> path)
	{
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
	}

	public bool IsInitialized()
	{
		return false;
	}

	public void BuyProduct(string id)
	{
	}

	public void BuyProductID(string productId)
	{
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		return default(PurchaseProcessingResult);
	}

	private bool IsVerification(PurchaseEventArgs args)
	{
		return false;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
	}

	public void RestorePurchase()
	{
	}

	private void OnPurchase(PaymentProductData product, PurchaseFailureReason failureReason, bool result)
	{
	}

	private KeyValuePair<string, string> GetPath()
	{
		return default(KeyValuePair<string, string>);
	}

	private void MyDebug(string debug)
	{
	}
}
