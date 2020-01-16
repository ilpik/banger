using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal class ReorderableListControl
	{
		internal delegate T ItemDrawer<T>(Rect position, T item);

		internal delegate void DrawEmpty();

		internal delegate void DrawEmptyAbsolute(Rect position);

		private struct ListInfo
		{
			public int ControlID;

			public Rect Position;

			public ListInfo(int controlID, Rect position)
			{
				ControlID = controlID;
				Position = position;
			}
		}

		private struct ItemInfo
		{
			public int ItemIndex;

			public Rect ItemPosition;

			public ItemInfo(int itemIndex, Rect itemPosition)
			{
				ItemIndex = itemIndex;
				ItemPosition = itemPosition;
			}
		}

		public static readonly Color AnchorBackgroundColor;

		public static readonly Color TargetBackgroundColor;

		private static GUIStyle s_RightAlignedLabelStyle;

		private static readonly int s_ReorderableListControlHint;

		private static float s_AnchorMouseOffset;

		private static int s_AnchorIndex;

		private static int s_TargetIndex;

		private static int s_AutoFocusControlID;

		private static int s_AutoFocusIndex;

		private static Stack<ListInfo> s_CurrentListStack;

		private static Stack<ItemInfo> s_CurrentItemStack;

		private ReorderableListFlags _flags;

		private float _verticalSpacing = 10f;

		private GUIStyle _containerStyle;

		private GUIStyle _footerButtonStyle;

		private GUIStyle _itemButtonStyle;

		private Color _horizontalLineColor;

		private bool _horizontalLineAtStart;

		private bool _horizontalLineAtEnd;

		private int _addMenuClickedSubscriberCount;

		private int _controlID;

		private Rect _visibleRect;

		private float _indexLabelWidth;

		private bool _tracking;

		private bool _allowReordering;

		private bool _allowDropInsertion;

		private int _insertionIndex;

		private float _insertionPosition;

		private int _newSizeInput;

		private static int s_SimulateMouseDragControlID;

		private static bool s_TrackingCancelBlockContext;

		private static Rect s_DragItemPosition;

		private static Rect s_RemoveButtonPosition;

		private static int s_DropTargetNestedCounter;

		private static Dictionary<int, float> s_ContainerHeightCache;

		private static readonly GUIContent s_Temp;

		private static readonly GUIContent s_SizePrefixLabel;

		protected static readonly GUIContent CommandMoveToTop;

		protected static readonly GUIContent CommandMoveToBottom;

		protected static readonly GUIContent CommandInsertAbove;

		protected static readonly GUIContent CommandInsertBelow;

		protected static readonly GUIContent CommandDuplicate;

		protected static readonly GUIContent CommandRemove;

		protected static readonly GUIContent CommandClearAll;

		private static int s_ContextControlID;

		private static int s_ContextItemIndex;

		private static string s_ContextCommandName;

		protected static readonly GenericMenu.MenuFunction2 DefaultContextHandler;

		public static int CurrentListControlID => s_CurrentListStack.Peek().ControlID;

		public static Rect CurrentListPosition => s_CurrentListStack.Peek().Position;

		internal static int CurrentItemIndex => s_CurrentItemStack.Peek().ItemIndex;

		public static Rect CurrentItemTotalPosition => s_CurrentItemStack.Peek().ItemPosition;

		public ReorderableListFlags Flags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}

		private bool HasFooterControls
		{
			get
			{
				if (!HasSizeField && !HasAddButton)
				{
					return HasAddMenuButton;
				}
				return true;
			}
		}

		private bool HasSizeField => (_flags & ReorderableListFlags.ShowSizeField) != 0;

		private bool HasAddButton => (_flags & ReorderableListFlags.HideAddButton) == 0;

		private bool HasAddMenuButton
		{
			get;
			set;
		}

		private bool HasRemoveButtons => (_flags & ReorderableListFlags.HideRemoveButtons) == 0;

		public float VerticalSpacing
		{
			get
			{
				return _verticalSpacing;
			}
			set
			{
				_verticalSpacing = value;
			}
		}

		public GUIStyle ContainerStyle
		{
			get
			{
				return _containerStyle;
			}
			set
			{
				_containerStyle = value;
			}
		}

		public GUIStyle FooterButtonStyle
		{
			get
			{
				return _footerButtonStyle;
			}
			set
			{
				_footerButtonStyle = value;
			}
		}

		public GUIStyle ItemButtonStyle
		{
			get
			{
				return _itemButtonStyle;
			}
			set
			{
				_itemButtonStyle = value;
			}
		}

		public Color HorizontalLineColor
		{
			get
			{
				return _horizontalLineColor;
			}
			set
			{
				_horizontalLineColor = value;
			}
		}

		public bool HorizontalLineAtStart
		{
			get
			{
				return _horizontalLineAtStart;
			}
			set
			{
				_horizontalLineAtStart = value;
			}
		}

		public bool HorizontalLineAtEnd
		{
			get
			{
				return _horizontalLineAtEnd;
			}
			set
			{
				_horizontalLineAtEnd = value;
			}
		}

		private event AddMenuClickedEventHandler _addMenuClicked;

		public event AddMenuClickedEventHandler AddMenuClicked
		{
			add
			{
				if (value != null)
				{
					_addMenuClicked += value;
					_addMenuClickedSubscriberCount++;
					HasAddMenuButton = (_addMenuClickedSubscriberCount != 0);
				}
			}
			remove
			{
				if (value != null)
				{
					_addMenuClicked -= value;
					_addMenuClickedSubscriberCount--;
					HasAddMenuButton = (_addMenuClickedSubscriberCount != 0);
				}
			}
		}

		public event ItemInsertedEventHandler ItemInserted;

		public event ItemRemovingEventHandler ItemRemoving;

		public event ItemMovingEventHandler ItemMoving;

		public event ItemMovedEventHandler ItemMoved;

		static ReorderableListControl()
		{
			s_ReorderableListControlHint = "_ReorderableListControl_".GetHashCode();
			s_AnchorIndex = -1;
			s_TargetIndex = -1;
			s_AutoFocusControlID = 0;
			s_AutoFocusIndex = -1;
			s_DropTargetNestedCounter = 0;
			s_ContainerHeightCache = new Dictionary<int, float>();
			s_Temp = new GUIContent();
			s_SizePrefixLabel = new GUIContent("Size");
			CommandMoveToTop = new GUIContent("Move to Top");
			CommandMoveToBottom = new GUIContent("Move to Bottom");
			CommandInsertAbove = new GUIContent("Insert Above");
			CommandInsertBelow = new GUIContent("Insert Below");
			CommandDuplicate = new GUIContent("Duplicate");
			CommandRemove = new GUIContent("Remove");
			CommandClearAll = new GUIContent("Clear All");
			DefaultContextHandler = DefaultContextMenuHandler;
			s_CurrentListStack = new Stack<ListInfo>();
			s_CurrentListStack.Push(default(ListInfo));
			s_CurrentItemStack = new Stack<ItemInfo>();
			s_CurrentItemStack.Push(new ItemInfo(-1, default(Rect)));
			if (EditorGUIUtility.isProSkin)
			{
				AnchorBackgroundColor = new Color(0.333333343f, 0.333333343f, 0.333333343f, 0.85f);
				TargetBackgroundColor = new Color(0f, 0f, 0f, 0.5f);
			}
			else
			{
				AnchorBackgroundColor = new Color(0.882352948f, 0.882352948f, 0.882352948f, 0.85f);
				TargetBackgroundColor = new Color(0f, 0f, 0f, 0.5f);
			}
		}

		private static int GetReorderableListControlID()
		{
			return GUIUtility.GetControlID(s_ReorderableListControlHint, FocusType.Passive);
		}

		public static void DrawControlFromState(IReorderableListAdaptor adaptor, DrawEmpty drawEmpty, ReorderableListFlags flags)
		{
			int reorderableListControlID = GetReorderableListControlID();
			ReorderableListControl obj = GUIUtility.GetStateObject(typeof(ReorderableListControl), reorderableListControlID) as ReorderableListControl;
			obj.Flags = flags;
			obj.Draw(reorderableListControlID, adaptor, drawEmpty);
		}

		public static void DrawControlFromState(Rect position, IReorderableListAdaptor adaptor, DrawEmptyAbsolute drawEmpty, ReorderableListFlags flags)
		{
			int reorderableListControlID = GetReorderableListControlID();
			ReorderableListControl obj = GUIUtility.GetStateObject(typeof(ReorderableListControl), reorderableListControlID) as ReorderableListControl;
			obj.Flags = flags;
			obj.Draw(position, reorderableListControlID, adaptor, drawEmpty);
		}

		protected virtual void OnAddMenuClicked(AddMenuClickedEventArgs args)
		{
			if (this._addMenuClicked != null)
			{
				this._addMenuClicked(this, args);
			}
		}

		protected virtual void OnItemInserted(ItemInsertedEventArgs args)
		{
			if (this.ItemInserted != null)
			{
				this.ItemInserted(this, args);
			}
		}

		protected virtual void OnItemRemoving(ItemRemovingEventArgs args)
		{
			if (this.ItemRemoving != null)
			{
				this.ItemRemoving(this, args);
			}
		}

		protected virtual void OnItemMoving(ItemMovingEventArgs args)
		{
			if (this.ItemMoving != null)
			{
				this.ItemMoving(this, args);
			}
		}

		protected virtual void OnItemMoved(ItemMovedEventArgs args)
		{
			if (this.ItemMoved != null)
			{
				this.ItemMoved(this, args);
			}
		}

		public ReorderableListControl()
		{
			_containerStyle = ReorderableListStyles.Container;
			_footerButtonStyle = ReorderableListStyles.FooterButton;
			_itemButtonStyle = ReorderableListStyles.ItemButton;
			_horizontalLineColor = ReorderableListStyles.HorizontalLineColor;
		}

		public ReorderableListControl(ReorderableListFlags flags)
			: this()
		{
			Flags = flags;
		}

		private void PrepareState(int controlID, IReorderableListAdaptor adaptor)
		{
			_controlID = controlID;
			_visibleRect = GUIHelper.VisibleRect();
			if ((Flags & ReorderableListFlags.ShowIndices) != 0)
			{
				_indexLabelWidth = CountDigits(adaptor.Count) * 8 + 8;
			}
			else
			{
				_indexLabelWidth = 0f;
			}
			_tracking = IsTrackingControl(controlID);
			_allowReordering = ((Flags & ReorderableListFlags.DisableReordering) == 0);
			_allowDropInsertion = true;
		}

		private static int CountDigits(int number)
		{
			return Mathf.Max(2, Mathf.CeilToInt(Mathf.Log10(number)));
		}

		private void AutoFocusItem(int controlID, int itemIndex)
		{
			if ((Flags & ReorderableListFlags.DisableAutoFocus) == 0)
			{
				s_AutoFocusControlID = controlID;
				s_AutoFocusIndex = itemIndex;
			}
		}

		private bool DoRemoveButton(Rect position, bool visible)
		{
			Texture2D texture = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_Remove_Normal);
			Texture2D texture2 = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_Remove_Active);
			return GUIHelper.IconButton(position, visible, texture, texture2, ItemButtonStyle);
		}

		private static void BeginTrackingReorderDrag(int controlID, int itemIndex)
		{
			GUIUtility.hotControl = controlID;
			GUIUtility.keyboardControl = 0;
			s_AnchorIndex = itemIndex;
			s_TargetIndex = itemIndex;
			s_TrackingCancelBlockContext = false;
		}

		private static void StopTrackingReorderDrag()
		{
			GUIUtility.hotControl = 0;
			s_AnchorIndex = -1;
			s_TargetIndex = -1;
		}

		private static bool IsTrackingControl(int controlID)
		{
			if (!s_TrackingCancelBlockContext)
			{
				return GUIUtility.hotControl == controlID;
			}
			return false;
		}

		private void AcceptReorderDrag(IReorderableListAdaptor adaptor)
		{
			try
			{
				s_TargetIndex = Mathf.Clamp(s_TargetIndex, 0, adaptor.Count + 1);
				if (s_TargetIndex != s_AnchorIndex && s_TargetIndex != s_AnchorIndex + 1)
				{
					MoveItem(adaptor, s_AnchorIndex, s_TargetIndex);
				}
			}
			finally
			{
				StopTrackingReorderDrag();
			}
		}

		private void DrawListItem(Rect position, IReorderableListAdaptor adaptor, int itemIndex)
		{
			bool flag = Event.current.type == EventType.Repaint;
			bool flag2 = position.y < _visibleRect.yMax && position.yMax > _visibleRect.y;
			bool flag3 = _allowReordering && adaptor.CanDrag(itemIndex);
			Rect position2 = position;
			position2.x = position.x + 2f;
			position2.y += 1f;
			position2.width = position.width - 4f;
			position2.height = position.height - 4f;
			if (flag3)
			{
				position2.x += 20f;
				position2.width -= 20f;
			}
			if (_indexLabelWidth != 0f)
			{
				position2.width -= _indexLabelWidth;
				if (flag && flag2)
				{
					s_RightAlignedLabelStyle.Draw(new Rect(position2.x, position.y, _indexLabelWidth, position.height - 4f), itemIndex + ":", isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
				}
				position2.x += _indexLabelWidth;
			}
			if (HasRemoveButtons)
			{
				position2.width -= 27f;
			}
			try
			{
				s_CurrentItemStack.Push(new ItemInfo(itemIndex, position));
				EditorGUI.BeginChangeCheck();
				if (flag && flag2)
				{
					Rect position3 = new Rect(position.x, position.y, position.width, position.height - 1f);
					adaptor.DrawItemBackground(position3, itemIndex);
					if (flag3)
					{
						GUIHelper.DrawTexture(new Rect(position.x + 6f, position.y + position.height / 2f - 3f, 9f, 5f), ReorderableListResources.GetTexture(ReorderableListTexture.GrabHandle));
					}
					if ((!_tracking || itemIndex != s_AnchorIndex) && (itemIndex != 0 || HorizontalLineAtStart))
					{
						GUIHelper.Separator(new Rect(position.x, position.y - 1f, position.width, 1f), HorizontalLineColor);
					}
				}
				if (s_AutoFocusIndex == itemIndex)
				{
					GUI.SetNextControlName("AutoFocus_" + _controlID + "_" + itemIndex);
				}
				adaptor.DrawItem(position2, itemIndex);
				if (EditorGUI.EndChangeCheck())
				{
					ReorderableListGUI.IndexOfChangedItem = itemIndex;
				}
				if (HasRemoveButtons && adaptor.CanRemove(itemIndex))
				{
					s_RemoveButtonPosition = position;
					s_RemoveButtonPosition.width = 27f;
					s_RemoveButtonPosition.x = position2.xMax + 2f;
					s_RemoveButtonPosition.y -= 1f;
					if (DoRemoveButton(s_RemoveButtonPosition, flag2))
					{
						RemoveItem(adaptor, itemIndex);
					}
				}
				if ((Flags & ReorderableListFlags.DisableContextMenu) == 0 && Event.current.GetTypeForControl(_controlID) == EventType.ContextClick && position.Contains(Event.current.mousePosition))
				{
					ShowContextMenu(itemIndex, adaptor);
					Event.current.Use();
				}
			}
			finally
			{
				s_CurrentItemStack.Pop();
			}
		}

		private void DrawFloatingListItem(IReorderableListAdaptor adaptor, float targetSlotPosition)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Color color = GUI.color;
				Rect position = s_DragItemPosition;
				position.y = targetSlotPosition - 1f;
				position.height = 1f;
				GUIHelper.Separator(position, HorizontalLineColor);
				float num = position.x -= 1f;
				num = (position.y += 1f);
				position.width += 2f;
				position.height = s_DragItemPosition.height - 1f;
				GUI.color = TargetBackgroundColor;
				GUIHelper.DrawTexture(position, EditorGUIUtility.whiteTexture);
				num = (s_DragItemPosition.x -= 1f);
				s_DragItemPosition.width += 2f;
				num = (s_DragItemPosition.height -= 1f);
				GUI.color = AnchorBackgroundColor;
				GUIHelper.DrawTexture(s_DragItemPosition, EditorGUIUtility.whiteTexture);
				num = (s_DragItemPosition.x += 1f);
				s_DragItemPosition.width -= 2f;
				num = (s_DragItemPosition.height += 1f);
				GUI.color = new Color(0f, 0f, 0f, 0.6f);
				position.y = s_DragItemPosition.y - 1f;
				position.height = 1f;
				GUIHelper.DrawTexture(position, EditorGUIUtility.whiteTexture);
				position.y += s_DragItemPosition.height;
				GUIHelper.DrawTexture(position, EditorGUIUtility.whiteTexture);
				GUI.color = color;
			}
			DrawListItem(s_DragItemPosition, adaptor, s_AnchorIndex);
		}

		private void DrawListContainerAndItems(Rect position, IReorderableListAdaptor adaptor)
		{
			int num = s_DropTargetNestedCounter;
			EventType typeForControl = Event.current.GetTypeForControl(_controlID);
			Vector2 mousePosition = Event.current.mousePosition;
			int num2 = s_TargetIndex;
			float num3 = position.y + (float)ContainerStyle.padding.top;
			float num4 = position.yMax - (float)ContainerStyle.padding.bottom - s_DragItemPosition.height + 1f;
			bool flag = typeForControl == EventType.MouseDrag;
			if (s_SimulateMouseDragControlID == _controlID && typeForControl == EventType.Repaint)
			{
				s_SimulateMouseDragControlID = 0;
				flag = true;
			}
			if (flag && _tracking)
			{
				if (mousePosition.y < num3)
				{
					num2 = 0;
				}
				else if (mousePosition.y >= position.yMax)
				{
					num2 = adaptor.Count;
				}
				s_DragItemPosition.y = Mathf.Clamp(mousePosition.y + s_AnchorMouseOffset, num3, num4);
			}
			switch (typeForControl)
			{
			case EventType.MouseDown:
				if (_tracking)
				{
					s_TrackingCancelBlockContext = true;
					Event.current.Use();
				}
				break;
			case EventType.MouseUp:
				if (_controlID == GUIUtility.hotControl)
				{
					if (!s_TrackingCancelBlockContext && _allowReordering)
					{
						AcceptReorderDrag(adaptor);
					}
					else
					{
						StopTrackingReorderDrag();
					}
					Event.current.Use();
				}
				break;
			case EventType.KeyDown:
				if (_tracking && Event.current.keyCode == KeyCode.Escape)
				{
					StopTrackingReorderDrag();
					Event.current.Use();
				}
				break;
			case EventType.ExecuteCommand:
				if (s_ContextControlID == _controlID)
				{
					int itemIndex = s_ContextItemIndex;
					try
					{
						DoCommand(s_ContextCommandName, itemIndex, adaptor);
						Event.current.Use();
					}
					finally
					{
						s_ContextControlID = 0;
						s_ContextItemIndex = 0;
					}
				}
				break;
			case EventType.Repaint:
				ContainerStyle.Draw(position, GUIContent.none, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
				break;
			}
			ReorderableListGUI.IndexOfChangedItem = -1;
			Rect position2 = new Rect(position.x + (float)ContainerStyle.padding.left, num3, position.width - (float)ContainerStyle.padding.horizontal, 0f);
			float targetSlotPosition = num4;
			_insertionIndex = 0;
			_insertionPosition = position2.yMax;
			float num5 = 0f;
			float num6 = 0f;
			int count = adaptor.Count;
			for (int i = 0; i < count; i++)
			{
				position2.y = position2.yMax;
				position2.height = 0f;
				num5 = position2.y - num6 / 2f;
				if (_tracking)
				{
					if (i == s_TargetIndex)
					{
						targetSlotPosition = position2.y;
						position2.y += s_DragItemPosition.height;
					}
					if (i == s_AnchorIndex)
					{
						continue;
					}
					position2.height = adaptor.GetItemHeight(i) + 4f;
					num6 = position2.height;
				}
				else
				{
					position2.height = adaptor.GetItemHeight(i) + 4f;
					num6 = position2.height;
					float num7 = position2.y + position2.height / 2f;
					if (mousePosition.y > num5 && mousePosition.y <= num7)
					{
						_insertionIndex = i;
						_insertionPosition = position2.y;
					}
				}
				if (_tracking && flag)
				{
					float num8 = position2.y + position2.height / 2f;
					if (s_TargetIndex < i)
					{
						if (s_DragItemPosition.yMax > num5 && s_DragItemPosition.yMax < num8)
						{
							num2 = i;
						}
					}
					else if (s_TargetIndex > i && s_DragItemPosition.y > num5 && s_DragItemPosition.y < num8)
					{
						num2 = i;
					}
				}
				DrawListItem(position2, adaptor, i);
				if (adaptor.Count < count)
				{
					count = adaptor.Count;
					i--;
				}
				else if (Event.current.type != EventType.Used && typeForControl == EventType.MouseDown && GUI.enabled && position2.Contains(mousePosition))
				{
					GUIUtility.keyboardControl = 0;
					if (_allowReordering && adaptor.CanDrag(i) && Event.current.button == 0)
					{
						s_DragItemPosition = position2;
						BeginTrackingReorderDrag(_controlID, i);
						s_AnchorMouseOffset = position2.y - mousePosition.y;
						s_TargetIndex = i;
						Event.current.Use();
					}
				}
			}
			if (HorizontalLineAtEnd)
			{
				GUIHelper.Separator(new Rect(position2.x, position.yMax - (float)ContainerStyle.padding.vertical, position2.width, 1f), HorizontalLineColor);
			}
			num5 = position.yMax - num6 / 2f;
			_allowDropInsertion = false;
			if (IsTrackingControl(_controlID))
			{
				if (flag)
				{
					if (s_DragItemPosition.yMax >= num5)
					{
						num2 = count;
					}
					s_TargetIndex = num2;
					if (typeForControl == EventType.MouseDrag)
					{
						Event.current.Use();
					}
				}
				DrawFloatingListItem(adaptor, targetSlotPosition);
			}
			else if (s_DropTargetNestedCounter == num)
			{
				if (Event.current.mousePosition.y >= num5)
				{
					_insertionIndex = adaptor.Count;
					_insertionPosition = position2.yMax;
				}
				_allowDropInsertion = true;
			}
			GUIUtility.GetControlID(FocusType.Keyboard);
			if (flag && (Flags & ReorderableListFlags.DisableAutoScroll) == 0 && IsTrackingControl(_controlID))
			{
				AutoScrollTowardsMouse();
			}
		}

		private static bool ContainsRect(Rect a, Rect b)
		{
			if (a.Contains(new Vector2(b.xMin, b.yMin)))
			{
				return a.Contains(new Vector2(b.xMax, b.yMax));
			}
			return false;
		}

		private void AutoScrollTowardsMouse()
		{
			Rect a = GUIHelper.VisibleRect();
			Vector2 vector = Event.current.mousePosition;
			Rect b = new Rect(vector.x - 8f, vector.y - 8f, 16f, 16f);
			if (!ContainsRect(a, b))
			{
				vector = ((vector.y < a.center.y) ? new Vector2(b.xMin, b.yMin) : new Vector2(b.xMax, b.yMax));
				vector.x = Mathf.Max(vector.x - 4f, b.xMax);
				vector.y = Mathf.Min(vector.y + 4f, b.yMax);
				GUI.ScrollTo(new Rect(vector.x, vector.y, 1f, 1f));
				s_SimulateMouseDragControlID = _controlID;
				EditorWindow focusedWindow = EditorWindow.focusedWindow;
				if (focusedWindow != null)
				{
					focusedWindow.Repaint();
				}
			}
		}

		private void HandleDropInsertion(Rect position, IReorderableListAdaptor adaptor)
		{
			IReorderableListDropTarget reorderableListDropTarget = adaptor as IReorderableListDropTarget;
			if (reorderableListDropTarget != null && _allowDropInsertion && reorderableListDropTarget.CanDropInsert(_insertionIndex))
			{
				s_DropTargetNestedCounter++;
				switch (Event.current.type)
				{
				case EventType.DragUpdated:
					DragAndDrop.visualMode = DragAndDropVisualMode.Move;
					DragAndDrop.activeControlID = _controlID;
					reorderableListDropTarget.ProcessDropInsertion(_insertionIndex);
					Event.current.Use();
					break;
				case EventType.DragPerform:
					reorderableListDropTarget.ProcessDropInsertion(_insertionIndex);
					DragAndDrop.AcceptDrag();
					DragAndDrop.activeControlID = 0;
					Event.current.Use();
					break;
				default:
					reorderableListDropTarget.ProcessDropInsertion(_insertionIndex);
					break;
				}
				if (DragAndDrop.activeControlID == _controlID && Event.current.type == EventType.Repaint)
				{
					DrawDropIndicator(new Rect(position.x, _insertionPosition - 2f, position.width, 3f));
				}
			}
		}

		protected virtual void DrawDropIndicator(Rect position)
		{
			GUIHelper.Separator(position);
		}

		private void CheckForAutoFocusControl()
		{
			if (Event.current.type != EventType.Used && s_AutoFocusControlID == _controlID)
			{
				s_AutoFocusControlID = 0;
				GUIHelper.FocusTextInControl("AutoFocus_" + _controlID + "_" + s_AutoFocusIndex);
				s_AutoFocusIndex = -1;
			}
		}

		private void DrawFooterControls(Rect position, IReorderableListAdaptor adaptor)
		{
			if (!HasFooterControls)
			{
				return;
			}
			Rect rect = new Rect(position.xMax - 30f, position.yMax - 1f, 30f, FooterButtonStyle.fixedHeight);
			Rect position2 = rect;
			Texture2D texture = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_AddMenu_Normal);
			Texture2D texture2 = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_AddMenu_Active);
			if (HasSizeField)
			{
				Rect position3 = new Rect(position.x, position.yMax + 1f, Mathf.Max(150f, position.width / 3f), 16f);
				DrawSizeFooterControl(position3, adaptor);
			}
			if (HasAddButton)
			{
				if (HasAddMenuButton)
				{
					position2.x = rect.xMax - 14f;
					position2.xMax = rect.xMax;
					texture = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_Menu_Normal);
					texture2 = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_Menu_Active);
					rect.width -= 5f;
					rect.x = position2.x - rect.width + 1f;
				}
				Texture2D texture3 = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_Add_Normal);
				Texture2D texture4 = ReorderableListResources.GetTexture(ReorderableListTexture.Icon_Add_Active);
				if (GUIHelper.IconButton(rect, visible: true, texture3, texture4, FooterButtonStyle))
				{
					GUIUtility.keyboardControl = 0;
					AddItem(adaptor);
				}
			}
			if (HasAddMenuButton && GUIHelper.IconButton(position2, visible: true, texture, texture2, FooterButtonStyle))
			{
				GUIUtility.keyboardControl = 0;
				Rect buttonPosition = rect;
				buttonPosition.xMax = position.xMax;
				OnAddMenuClicked(new AddMenuClickedEventArgs(adaptor, buttonPosition));
				GUIUtility.ExitGUI();
			}
		}

		private void DrawSizeFooterControl(Rect position, IReorderableListAdaptor adaptor)
		{
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 60f;
			DrawSizeField(position, adaptor);
			EditorGUIUtility.labelWidth = labelWidth;
		}

		private Rect GetListRectWithAutoLayout(IReorderableListAdaptor adaptor, float padding)
		{
			float num;
			if (Event.current.type == EventType.Layout)
			{
				num = CalculateListHeight(adaptor);
				s_ContainerHeightCache[_controlID] = num;
			}
			else
			{
				num = (s_ContainerHeightCache.ContainsKey(_controlID) ? s_ContainerHeightCache[_controlID] : 0f);
			}
			num += padding;
			return GUILayoutUtility.GetRect(GUIContent.none, ContainerStyle, GUILayout.Height(num));
		}

		private Rect DrawLayoutListField(IReorderableListAdaptor adaptor, float padding)
		{
			Rect listRectWithAutoLayout = GetListRectWithAutoLayout(adaptor, padding);
			if (HasFooterControls)
			{
				listRectWithAutoLayout.height -= FooterButtonStyle.fixedHeight;
			}
			listRectWithAutoLayout.height -= VerticalSpacing;
			s_CurrentListStack.Push(new ListInfo(_controlID, listRectWithAutoLayout));
			try
			{
				adaptor.BeginGUI();
				DrawListContainerAndItems(listRectWithAutoLayout, adaptor);
				HandleDropInsertion(listRectWithAutoLayout, adaptor);
				adaptor.EndGUI();
			}
			finally
			{
				s_CurrentListStack.Pop();
			}
			CheckForAutoFocusControl();
			return listRectWithAutoLayout;
		}

		private Rect DrawLayoutEmptyList(IReorderableListAdaptor adaptor, DrawEmpty drawEmpty)
		{
			Rect rect = EditorGUILayout.BeginVertical(ContainerStyle);
			if (drawEmpty != null)
			{
				drawEmpty();
			}
			else
			{
				Debug.LogError("Unexpected call to 'DrawLayoutEmptyList'");
			}
			s_CurrentListStack.Push(new ListInfo(_controlID, rect));
			try
			{
				adaptor.BeginGUI();
				_insertionIndex = 0;
				_insertionPosition = rect.y + 2f;
				HandleDropInsertion(rect, adaptor);
				adaptor.EndGUI();
			}
			finally
			{
				s_CurrentListStack.Pop();
			}
			EditorGUILayout.EndVertical();
			if (HasFooterControls)
			{
				GUILayoutUtility.GetRect(0f, FooterButtonStyle.fixedHeight - 1f);
			}
			return rect;
		}

		private void DrawEmptyListControl(Rect position, DrawEmptyAbsolute drawEmpty)
		{
			if (Event.current.type == EventType.Repaint)
			{
				ContainerStyle.Draw(position, GUIContent.none, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
			}
			position = ContainerStyle.padding.Remove(position);
			drawEmpty?.Invoke(position);
		}

		private void FixStyles()
		{
			ContainerStyle = (ContainerStyle ?? ReorderableListStyles.Container);
			FooterButtonStyle = (FooterButtonStyle ?? ReorderableListStyles.FooterButton);
			ItemButtonStyle = (ItemButtonStyle ?? ReorderableListStyles.ItemButton);
			if (s_RightAlignedLabelStyle == null)
			{
				s_RightAlignedLabelStyle = new GUIStyle(GUI.skin.label);
				s_RightAlignedLabelStyle.alignment = TextAnchor.MiddleRight;
				s_RightAlignedLabelStyle.padding.right = 4;
			}
		}

		private void Draw(int controlID, IReorderableListAdaptor adaptor, DrawEmpty drawEmpty)
		{
			FixStyles();
			PrepareState(controlID, adaptor);
			Rect position = (adaptor.Count > 0) ? DrawLayoutListField(adaptor, 0f) : ((drawEmpty != null) ? DrawLayoutEmptyList(adaptor, drawEmpty) : DrawLayoutListField(adaptor, 5f));
			DrawFooterControls(position, adaptor);
		}

		public void Draw(IReorderableListAdaptor adaptor, DrawEmpty drawEmpty)
		{
			int reorderableListControlID = GetReorderableListControlID();
			Draw(reorderableListControlID, adaptor, drawEmpty);
		}

		public void Draw(IReorderableListAdaptor adaptor)
		{
			int reorderableListControlID = GetReorderableListControlID();
			Draw(reorderableListControlID, adaptor, null);
		}

		private void Draw(Rect position, int controlID, IReorderableListAdaptor adaptor, DrawEmptyAbsolute drawEmpty)
		{
			FixStyles();
			PrepareState(controlID, adaptor);
			if (HasFooterControls)
			{
				position.height -= FooterButtonStyle.fixedHeight;
			}
			position.height -= VerticalSpacing;
			s_CurrentListStack.Push(new ListInfo(_controlID, position));
			try
			{
				adaptor.BeginGUI();
				DrawListContainerAndItems(position, adaptor);
				HandleDropInsertion(position, adaptor);
				CheckForAutoFocusControl();
				if (adaptor.Count == 0)
				{
					ReorderableListGUI.IndexOfChangedItem = -1;
					DrawEmptyListControl(position, drawEmpty);
				}
				adaptor.EndGUI();
			}
			finally
			{
				s_CurrentListStack.Pop();
			}
			DrawFooterControls(position, adaptor);
		}

		public void Draw(Rect position, IReorderableListAdaptor adaptor, DrawEmptyAbsolute drawEmpty)
		{
			int reorderableListControlID = GetReorderableListControlID();
			Draw(position, reorderableListControlID, adaptor, drawEmpty);
		}

		public void Draw(Rect position, IReorderableListAdaptor adaptor)
		{
			int reorderableListControlID = GetReorderableListControlID();
			Draw(position, reorderableListControlID, adaptor, null);
		}

		public void DrawSizeField(Rect position, GUIContent label, IReorderableListAdaptor adaptor)
		{
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			string text = "ReorderableListControl.Size." + controlID;
			GUI.SetNextControlName(text);
			if (GUI.GetNameOfFocusedControl() == text)
			{
				if (Event.current.rawType == EventType.KeyDown)
				{
					KeyCode keyCode = Event.current.keyCode;
					if (keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter)
					{
						ResizeList(adaptor, _newSizeInput);
						Event.current.Use();
					}
				}
				_newSizeInput = EditorGUI.IntField(position, label, _newSizeInput);
			}
			else
			{
				EditorGUI.IntField(position, label, adaptor.Count);
				_newSizeInput = adaptor.Count;
			}
		}

		public void DrawSizeField(Rect position, string label, IReorderableListAdaptor adaptor)
		{
			s_Temp.text = label;
			DrawSizeField(position, s_Temp, adaptor);
		}

		public void DrawSizeField(Rect position, IReorderableListAdaptor adaptor)
		{
			DrawSizeField(position, s_SizePrefixLabel, adaptor);
		}

		public void DrawSizeField(GUIContent label, IReorderableListAdaptor adaptor)
		{
			Rect rect = GUILayoutUtility.GetRect(0f, EditorGUIUtility.singleLineHeight);
			DrawSizeField(rect, label, adaptor);
		}

		public void DrawSizeField(string label, IReorderableListAdaptor adaptor)
		{
			s_Temp.text = label;
			DrawSizeField(s_Temp, adaptor);
		}

		public void DrawSizeField(IReorderableListAdaptor adaptor)
		{
			DrawSizeField(s_SizePrefixLabel, adaptor);
		}

		private void ShowContextMenu(int itemIndex, IReorderableListAdaptor adaptor)
		{
			GenericMenu genericMenu = new GenericMenu();
			s_ContextControlID = _controlID;
			s_ContextItemIndex = itemIndex;
			AddItemsToMenu(genericMenu, itemIndex, adaptor);
			if (genericMenu.GetItemCount() > 0)
			{
				genericMenu.ShowAsContext();
			}
		}

		private static void DefaultContextMenuHandler(object userData)
		{
			GUIContent gUIContent = userData as GUIContent;
			if (gUIContent != null && !string.IsNullOrEmpty(gUIContent.text))
			{
				s_ContextCommandName = gUIContent.text;
				Event e = EditorGUIUtility.CommandEvent("ReorderableListContextCommand");
				EditorWindow.focusedWindow.SendEvent(e);
			}
		}

		protected virtual void AddItemsToMenu(GenericMenu menu, int itemIndex, IReorderableListAdaptor adaptor)
		{
			if ((Flags & ReorderableListFlags.DisableReordering) == 0)
			{
				if (itemIndex > 0)
				{
					menu.AddItem(CommandMoveToTop, on: false, DefaultContextHandler, CommandMoveToTop);
				}
				else
				{
					menu.AddDisabledItem(CommandMoveToTop);
				}
				if (itemIndex + 1 < adaptor.Count)
				{
					menu.AddItem(CommandMoveToBottom, on: false, DefaultContextHandler, CommandMoveToBottom);
				}
				else
				{
					menu.AddDisabledItem(CommandMoveToBottom);
				}
				if (HasAddButton)
				{
					menu.AddSeparator("");
					menu.AddItem(CommandInsertAbove, on: false, DefaultContextHandler, CommandInsertAbove);
					menu.AddItem(CommandInsertBelow, on: false, DefaultContextHandler, CommandInsertBelow);
					if ((Flags & ReorderableListFlags.DisableDuplicateCommand) == 0)
					{
						menu.AddItem(CommandDuplicate, on: false, DefaultContextHandler, CommandDuplicate);
					}
				}
			}
			if (HasRemoveButtons)
			{
				if (menu.GetItemCount() > 0)
				{
					menu.AddSeparator("");
				}
				menu.AddItem(CommandRemove, on: false, DefaultContextHandler, CommandRemove);
				menu.AddSeparator("");
				menu.AddItem(CommandClearAll, on: false, DefaultContextHandler, CommandClearAll);
			}
		}

		protected virtual bool HandleCommand(string commandName, int itemIndex, IReorderableListAdaptor adaptor)
		{
			switch (commandName)
			{
			case "Move to Top":
				MoveItem(adaptor, itemIndex, 0);
				return true;
			case "Move to Bottom":
				MoveItem(adaptor, itemIndex, adaptor.Count);
				return true;
			case "Insert Above":
				InsertItem(adaptor, itemIndex);
				return true;
			case "Insert Below":
				InsertItem(adaptor, itemIndex + 1);
				return true;
			case "Duplicate":
				DuplicateItem(adaptor, itemIndex);
				return true;
			case "Remove":
				RemoveItem(adaptor, itemIndex);
				return true;
			case "Clear All":
				ClearAll(adaptor);
				return true;
			default:
				return false;
			}
		}

		public bool DoCommand(string commandName, int itemIndex, IReorderableListAdaptor adaptor)
		{
			if (!HandleCommand(s_ContextCommandName, itemIndex, adaptor))
			{
				Debug.LogWarning("Unknown context command.");
				return false;
			}
			return true;
		}

		public bool DoCommand(GUIContent command, int itemIndex, IReorderableListAdaptor adaptor)
		{
			return DoCommand(command.text, itemIndex, adaptor);
		}

		public float CalculateListHeight(IReorderableListAdaptor adaptor)
		{
			FixStyles();
			float num = (float)(ContainerStyle.padding.vertical - 1) + VerticalSpacing;
			int count = adaptor.Count;
			for (int i = 0; i < count; i++)
			{
				num += adaptor.GetItemHeight(i);
			}
			num += (float)(4 * count);
			if (HasFooterControls)
			{
				num += FooterButtonStyle.fixedHeight;
			}
			return num;
		}

		public float CalculateListHeight(int itemCount, float itemHeight)
		{
			FixStyles();
			float num = (float)(ContainerStyle.padding.vertical - 1) + VerticalSpacing;
			num += (itemHeight + 4f) * (float)itemCount;
			if (HasFooterControls)
			{
				num += FooterButtonStyle.fixedHeight;
			}
			return num;
		}

		protected void MoveItem(IReorderableListAdaptor adaptor, int sourceIndex, int destIndex)
		{
			ItemMovingEventArgs itemMovingEventArgs = new ItemMovingEventArgs(adaptor, sourceIndex, destIndex);
			OnItemMoving(itemMovingEventArgs);
			if (!itemMovingEventArgs.Cancel)
			{
				adaptor.Move(sourceIndex, destIndex);
				int num = destIndex;
				if (num > sourceIndex)
				{
					num--;
				}
				OnItemMoved(new ItemMovedEventArgs(adaptor, sourceIndex, num));
				GUI.changed = true;
			}
			ReorderableListGUI.IndexOfChangedItem = -1;
		}

		protected void AddItem(IReorderableListAdaptor adaptor)
		{
			adaptor.Add();
			AutoFocusItem(s_ContextControlID, adaptor.Count - 1);
			GUI.changed = true;
			ReorderableListGUI.IndexOfChangedItem = -1;
			ItemInsertedEventArgs args = new ItemInsertedEventArgs(adaptor, adaptor.Count - 1, wasDuplicated: false);
			OnItemInserted(args);
		}

		protected void InsertItem(IReorderableListAdaptor adaptor, int itemIndex)
		{
			adaptor.Insert(itemIndex);
			AutoFocusItem(s_ContextControlID, itemIndex);
			GUI.changed = true;
			ReorderableListGUI.IndexOfChangedItem = -1;
			ItemInsertedEventArgs args = new ItemInsertedEventArgs(adaptor, itemIndex, wasDuplicated: false);
			OnItemInserted(args);
		}

		protected void DuplicateItem(IReorderableListAdaptor adaptor, int itemIndex)
		{
			adaptor.Duplicate(itemIndex);
			AutoFocusItem(s_ContextControlID, itemIndex + 1);
			GUI.changed = true;
			ReorderableListGUI.IndexOfChangedItem = -1;
			ItemInsertedEventArgs args = new ItemInsertedEventArgs(adaptor, itemIndex + 1, wasDuplicated: true);
			OnItemInserted(args);
		}

		protected bool RemoveItem(IReorderableListAdaptor adaptor, int itemIndex)
		{
			ItemRemovingEventArgs itemRemovingEventArgs = new ItemRemovingEventArgs(adaptor, itemIndex);
			OnItemRemoving(itemRemovingEventArgs);
			if (itemRemovingEventArgs.Cancel)
			{
				return false;
			}
			adaptor.Remove(itemIndex);
			GUI.changed = true;
			ReorderableListGUI.IndexOfChangedItem = -1;
			return true;
		}

		protected bool ClearAll(IReorderableListAdaptor adaptor)
		{
			if (adaptor.Count == 0)
			{
				return true;
			}
			ItemRemovingEventArgs itemRemovingEventArgs = new ItemRemovingEventArgs(adaptor, 0);
			int count = adaptor.Count;
			for (int i = 0; i < count; i++)
			{
				itemRemovingEventArgs.ItemIndex = i;
				OnItemRemoving(itemRemovingEventArgs);
				if (itemRemovingEventArgs.Cancel)
				{
					return false;
				}
			}
			adaptor.Clear();
			GUI.changed = true;
			ReorderableListGUI.IndexOfChangedItem = -1;
			return true;
		}

		protected bool ResizeList(IReorderableListAdaptor adaptor, int newCount)
		{
			if (newCount < 0)
			{
				return true;
			}
			int num = Mathf.Max(0, adaptor.Count - newCount);
			int num2 = Mathf.Max(0, newCount - adaptor.Count);
			while (num-- > 0)
			{
				if (!RemoveItem(adaptor, adaptor.Count - 1))
				{
					return false;
				}
			}
			while (num2-- > 0)
			{
				AddItem(adaptor);
			}
			return true;
		}
	}
}
